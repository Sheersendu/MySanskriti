using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentService.API.DTOs;
using PaymentService.Application.UseCases;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("/api/payment")]
public class PaymentServiceController : ControllerBase
{
	private readonly CreatePaymentHandler _createPaymentHandler;
	private readonly GetPaymentsForUserIdHandler _getPaymentsForUserIdHandler;
	
	public PaymentServiceController(CreatePaymentHandler createPaymentHandler, GetPaymentsForUserIdHandler getPaymentsForUserIdHandler)
	{
		_createPaymentHandler = createPaymentHandler;
		_getPaymentsForUserIdHandler = getPaymentsForUserIdHandler;
		StripeConfiguration.ApiKey = "";
	}
	
		
	[HttpGet]
	public async Task<ActionResult> GetPaymentDetails([FromQuery] Guid userId)
	{
		return Ok(await _getPaymentsForUserIdHandler.Handle(userId));
	}
	
	[HttpPost("create-checkout-session")]
	public ActionResult CreateCheckoutSession([FromBody] PaymentRequest paymentRequest)
	{
		var customerOptions = new CustomerCreateOptions
		{
			Name = "John Doe",
			Address = new AddressOptions
			{
				Country = "India"
			}
		};

		var customerService = new CustomerService();
		var customer = customerService.Create(customerOptions);

		var options = new SessionCreateOptions
		{
			Customer = customer.Id,
			LineItems =
			[
				new()
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)paymentRequest.amount * 100, // Stripe expects amount in cents hence multiply by 100
						Currency = "inr",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = "Event Name",
							Description = "Event description"
						},
					},
					Quantity = 1,
				}

			],
			Mode = "payment",
			SuccessUrl = "http://localhost:4242/success",
			CancelUrl = "http://localhost:4242/cancel",
			PaymentIntentData = new SessionPaymentIntentDataOptions
			{
				Metadata = new Dictionary<string, string>
				{
					{ "booking_id", paymentRequest.bookingId.ToString() }
				}
			}
		};

		var sessionService = new SessionService();
		var session = sessionService.Create(options);

		// Uncomment the following lines to redirect to Stripe's hosted checkout page in RL
		// Response.Headers.Add("Location", session.Url); // needs to be sent to FE as it redirects user to Stripe’s hosted checkout page
		// return new StatusCodeResult(303);
		
		// Local testing
		return Ok(new { url = session.Url });
	}
	
	[HttpPost("webhook-endpoint")]
	public async Task<IActionResult> StripeWebhook()
	{
		using var reader = new StreamReader(HttpContext.Request.Body);
		var json = await reader.ReadToEndAsync();

		var stripeEvent = EventUtility.ConstructEvent(
			json,
			Request.Headers["Stripe-Signature"],
			""
		);

		switch (stripeEvent.Type)
		{
			case "checkout.session.completed":
				var session = stripeEvent.Data.Object as Session;
				if (session != null)
				{
					var paymentIntentId = session.PaymentIntentId;
					var paymentIntentService = new PaymentIntentService();
					var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);
					var bookingIdStr = paymentIntent.Metadata["booking_id"];
					var amount = session.AmountTotal/100 ?? 0;

					await _createPaymentHandler.Handle(
						amount: amount,
						bookingId: Guid.TryParse(bookingIdStr, out var bid) ? bid : Guid.NewGuid(),
						transactionId: paymentIntentId,
						createdAt: DateTime.UtcNow,
						paymentStatus: PaymentStatus.Completed
					);
					Console.WriteLine($"Payment succeeded for Booking ID: {bookingIdStr}");
				}
				break;

			case "payment_intent.payment_failed":
				var intent = stripeEvent.Data.Object as PaymentIntent;
				var bookingId = intent?.Metadata["booking_id"];
				var failedAmount = intent?.Amount/100 ?? 0;

				await _createPaymentHandler.Handle(
					amount: failedAmount,
					bookingId: Guid.TryParse(bookingId, out var failedBid) ? failedBid : Guid.NewGuid(),
					transactionId: intent.Id,
					createdAt: DateTime.UtcNow,
					paymentStatus: PaymentStatus.Failed
				);
				Console.WriteLine($"Payment failed. Booking ID: {bookingId}");
				break;
		}

		return Ok();
	}

}