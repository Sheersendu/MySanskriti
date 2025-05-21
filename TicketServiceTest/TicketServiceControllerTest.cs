using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketService.API.Controllers;
using TicketService.API.DTOs;
using TicketService.API.Mappings;
using TicketService.Application.Exceptions;
using TicketService.Application.Interfaces;
using TicketService.Application.UseCases;
using TicketService.Domain.Entities;
using TicketService.Domain.Enums;

namespace TicketServiceTest;

public class TicketServiceControllerTest
{
	private readonly TicketController _ticketController;
	private readonly Mock<ITicketRepository> _ticketRepository;
	
	public TicketServiceControllerTest()
	{
		_ticketRepository = new Mock<ITicketRepository>();
		GetTicketHandler getTicketHandler = new (_ticketRepository.Object);
		CreateTicketHandler createTicketHandler = new (_ticketRepository.Object);
		CancelTicketHandler cancelTicketHandler = new (_ticketRepository.Object);
		MapperConfiguration configuration = new (cfg =>
		{
			cfg.AddProfile<TicketResponseMapping>();
		});
		IMapper? mapper = configuration.CreateMapper();
		
		_ticketController = new TicketController(getTicketHandler, createTicketHandler, cancelTicketHandler, mapper);
	}

	[Test]
	public void TestGetTicketByBookingId_WhenValidBookingId_ShouldReturnTicket()
	{
		// Arrange
		var bookingId = Guid.NewGuid();
		var ticket = new Ticket();
		
		_ticketRepository.Setup(repo => repo.GetTicketByBookingId(bookingId)).ReturnsAsync(ticket);
		
		// Act
		var result = _ticketController.GetTicket(bookingId).Result;
		
		// Assert
		Assert.That(result, Is.Not.Null);
		OkObjectResult? okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);
		TicketResponse? actualTicket = okResult.Value as TicketResponse;
		Assert.That(actualTicket.ticketId, Is.EqualTo(ticket.TicketId));
	}
	
	[Test]
	public void TestGetTicketByBookingId_WhenInvalidBookingId_ShouldThrowBookingNotFoundException()
	{
		// Arrange
		var bookingId = Guid.NewGuid();
		var exceptionMessage = $"No ticket for Booking ID: `{bookingId}` was found";
		_ticketRepository.Setup(repo => repo.GetTicketByBookingId(bookingId)).Throws(new BookingNotFoundException(exceptionMessage));
		
		// Act
		var result = _ticketController.GetTicket(bookingId).Result;
		
		// Assert
		Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
		NotFoundObjectResult? notFoundResult = result as NotFoundObjectResult;
		Assert.That(notFoundResult, Is.Not.Null);
		Assert.That(notFoundResult.Value?.ToString(), Is.EqualTo(exceptionMessage));
	}
	
	[Test]
	public void TestCreateTicket_ShouldCreateAndReturnNewTicket()
	{
		// Arrange
		var bookingId = Guid.NewGuid();
		TicketRequest ticketRequest = new()
		{
			bookingId = bookingId
		};
		Ticket newTicket = new()
		{
			BookingId = bookingId,
			TicketId = Guid.NewGuid(),
			CreatedTimestamp = DateTime.Now,
			Status = TicketStatus.CONFIRMED
		};
		_ticketRepository.Setup(repo => repo.CreateTicket(It.IsAny<Ticket>())).ReturnsAsync(newTicket);
		
		// Act
		var result = _ticketController.CreateTicket(ticketRequest).Result;
		
		// Assert
		Assert.That(result, Is.Not.Null);
		OkObjectResult? okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);
		TicketResponse? newCreatedTicket = okResult.Value as TicketResponse;
		Assert.That(newCreatedTicket.bookingId, Is.EqualTo(bookingId));
		Assert.That(newCreatedTicket.ticketStatus, Is.EqualTo(TicketStatus.CONFIRMED.ToString()));
	}
	
	[Test]
	public void TestCancelTicket_WhenValidBookingID_ShouldCancelTicket()
	{
		// Arrange
		var bookingId = Guid.NewGuid();
		var ticketId = Guid.NewGuid();
		var createdTimestamp = DateTime.Now;
		TicketRequest ticketRequest = new()
		{
			bookingId = bookingId
		};
		Ticket existingTicket = new()
		{
			BookingId = bookingId,
			TicketId = ticketId,
			CreatedTimestamp = createdTimestamp,
			Status = TicketStatus.CONFIRMED
		};
		Ticket cancelledTicket = new()
		{
			BookingId = bookingId,
			TicketId = ticketId,
			CreatedTimestamp = createdTimestamp,
			Status = TicketStatus.CANCELLED
		};
		_ticketRepository.Setup(repo => repo.GetTicketByBookingId(bookingId)).ReturnsAsync(existingTicket);
		_ticketRepository.Setup(repo => repo.CancelTicket(It.IsAny<Ticket>())).ReturnsAsync(cancelledTicket);
		
		// Act
		var result = _ticketController.CancelTicket(ticketRequest).Result;
		
		// Assert
		Assert.That(result, Is.Not.Null);
		OkObjectResult? okResult = result as OkObjectResult;
		Assert.That(okResult, Is.Not.Null);
		TicketResponse? cancelTicket = okResult.Value as TicketResponse;
		Assert.That(cancelTicket.bookingId, Is.EqualTo(bookingId));
		Assert.That(cancelTicket.ticketId, Is.EqualTo(ticketId));
		Assert.That(cancelTicket.ticketStatus, Is.EqualTo(TicketStatus.CANCELLED.ToString()));
	}
	
	[Test]
	public void TestCancelTicket_WhenCancelledBooking_ShouldReturnTicketAlreadyCancelledException()
	{
		// Arrange
		var bookingId = Guid.NewGuid();
		var ticketId = Guid.NewGuid();
		var createdTimestamp = DateTime.Now;
		const string exceptionMessage = "Ticket is already cancelled.";
		TicketRequest ticketRequest = new()
		{
			bookingId = bookingId
		};
		Ticket cancelledTicket = new()
		{
			BookingId = bookingId,
			TicketId = ticketId,
			CreatedTimestamp = createdTimestamp,
			Status = TicketStatus.CANCELLED
		};
		_ticketRepository.Setup(repo => repo.GetTicketByBookingId(bookingId)).ReturnsAsync(cancelledTicket);
		
		// Act
		var result = _ticketController.CancelTicket(ticketRequest).Result;
		
		// Assert
		Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
		BadRequestObjectResult? badResult = result as BadRequestObjectResult;
		Assert.That(badResult, Is.Not.Null);
		Assert.That(badResult.Value?.ToString(), Is.EqualTo(exceptionMessage));
	}
}