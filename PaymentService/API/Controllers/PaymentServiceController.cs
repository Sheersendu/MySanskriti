using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentService.API.DTOs;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("/api/payment")]
public class PaymentServiceController : ControllerBase
{
	public PaymentServiceController()
	{
		StripeConfiguration.ApiKey = "";
	}
	
		
	[HttpGet]
	public string GetPaymentDetails()
	{
		return "Payment details retrieved successfully.";
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
			Metadata = new Dictionary<string, string>
			{
				{"booking_id", paymentRequest.bookingId.ToString()}
			},
			LineItems =
			[
				new()
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)paymentRequest.amount * 100, // Stripe expects amount in cents
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = "Very costly stuff",
							Description = "Someone's dream!"
						},
					},
					Quantity = 1,
				}

			],
			Mode = "payment",
			SuccessUrl = "http://localhost:4242/success",
			CancelUrl = "http://localhost:4242/cancel",
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

		
		if (stripeEvent.Type == "payment_intent.succeeded") // payment_failed
		{
			var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
			var paymentAmount = paymentIntent?.Amount ?? 0;
			Console.WriteLine($"Payment for {paymentAmount} succeeded.");
		}
		if (stripeEvent.Type == "checkout.session.completed")
		{
			var session = stripeEvent.Data.Object as Session;
			var bookingId = session.Metadata["booking_id"];
			Console.WriteLine($"Booking ID: {bookingId} - Checkout session completed successfully.");
		}


		return Ok();
	}

}