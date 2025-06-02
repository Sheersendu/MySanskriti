using System.Text.Json;
using PaymentService.API.DTOs;
using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.Persistence;

public class TicketClient(HttpClient httpClient) : ITicketClient
{
	public async Task<TicketDTO> GenerateTicket(Guid bookingId)
	{
		var endpoint = "/api/ticket/create";
		var requestBody = new
		{
			bookingId = bookingId.ToString()
		};
		var response = await httpClient.PostAsync(endpoint, new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json"));
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<TicketDTO>();
	}
}