using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
	public ActionResult CreateCheckoutSession()
	{
		var customerOptions = new CustomerCreateOptions
		{
			Name = "John Doe",
			Address = new AddressOptions
			{
				Line1 = "123 MG Road",
				City = "Mumbai",
				State = "MH",
				PostalCode = "400001",
				Country = "IN"
			}
		};

		var customerService = new CustomerService();
		var customer = customerService.Create(customerOptions);

		var options = new SessionCreateOptions
		{
			Customer = customer.Id, // pass customer ID here
			LineItems = new List<SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = 1235,
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = "Very costly stuff",
							Description = "Someone's dream!"
						},
					},
					Quantity = 1,
				},
			},
			Mode = "payment",
			SuccessUrl = "http://localhost:4242/success",
			CancelUrl = "http://localhost:4242/cancel",
		};

		var sessionService = new SessionService();
		var session = sessionService.Create(options);

		Response.Headers.Add("Location", session.Url); // needs to be sent to FE as it redirects user to Stripe’s hosted checkout page
		return new StatusCodeResult(303); // redirect

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

		
		if (stripeEvent.Type.ToString() == "payment_intent.succeeded")
		{
			var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
			Console.WriteLine($"Payment for {paymentIntent.Amount} succeeded.");
			// Process business logic here
		}

		return Ok();
	}

}