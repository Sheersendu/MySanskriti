# MySanskriti
### An event booking system for all religious events like pravachans, kathas and more in a location

### Features
1. User Registration and Login
2. Home page shows a list of events based on user's selected city
3. Default city is set to "Bengaluru"
4. User will be able to book tickets for an event
5. Event types: kathas, pujas, pravachan, satsang, special event
6. User will be able to select seat and book
7. User will be able to view booked tickets
8. User will get ticket confirmation via email/SMS
9. User will be able to search and filter events based on event type, date and location
10. Organizations/Individuals(event organizers) can register and create events
11. Event Organizers can view their events and manage them(Update, Delete)
12. User can make payment
13. User can view booking history

### Good to have

| Feature                                                   | Why Add It                                                                                         |
| --------------------------------------------------------- | -------------------------------------------------------------------------------------------------- |
| **14. Cancel Booked Tickets (with/without refund logic)** | Useful in real event flows; introduces interesting time-based rules (e.g., “Cancel before 24 hrs”) |
| **15. Save Events to Favorites / Reminders**              | Simple enhancement that helps user retention. Could integrate with calendar/notifications later.   |
| **16. Ratings & Feedback for Events**                     | Enables community trust and quality. Useful if your app scales to many organizers.                 |

Entities:
1. User
2. Event
3. Payment
4. Seat
5. Location
6. Ticket
7. Booking

Relations:
1. User has booking
2. User selects seat(s)
3. Payment has booking
4. Booking has event and seat
5. Event has location, seat
6. Ticket has booking (details), Payment

#### NOTE:

<details>
<summary>📌 Redis Eviction Policy</summary>

By default, Redis won't evict anything unless it’s configured to do so. Run Redis Docker (everytime!) with:

```
docker run -d --name my-redis -p 6379:6379 redis

1. go into redis-cli ( terminal )
redis-cli

2. setting maxmemory and eviction policy
CONFIG SET maxmemory 100mb
CONFIG SET maxmemory-policy allkeys-lru

3. verifying eviction policy
CONFIG GET maxmemory
CONFIG GET maxmemory-policy

Explanation:
- maxmemory 100mb: Limits Redis to 100 MB.
- maxmemory-policy allkeys-lru: Enables LRU eviction across all keys.
```

</details>
<details>
<summary>📌 Step-by-Step Stripe Payment Flow (ChatGPT)</summary>

1. User initiates payment (Frontend)

    The user clicks a “Pay” or “Buy” button.

    This triggers a call to your backend API to create a Stripe Checkout session.

✅ Why backend?

    Sensitive data like product price, currency, and Stripe secret keys must not be exposed in frontend.

    You may want to validate the cart, compute price, apply discounts, and save a pending order in DB before payment.

2. Backend calls Stripe API to create a Checkout session

   Your backend (Node, .NET, Python, etc.) calls Stripe using your secret key to create a session.

   You specify:

        Products (name, price, quantity)

        Payment method

        Success/cancel URLs

        Optionally customer details (name, email, etc.)

        (in India) Address and name for export compliance

   Stripe returns a session URL, which you send back to the frontend.


3. Frontend redirects user to Stripe’s hosted checkout page

   The frontend uses window.location.href = session.url to redirect.

   Stripe handles card input, UPI, and all sensitive payment processing on its secure page.

✅ You don’t touch any card data, avoiding PCI compliance issues.
4. Stripe processes the payment

   If payment succeeds, the user is redirected to the success_url.

   If cancelled, Stripe redirects to the cancel_url.

🚫 ⚠️ At this point, you cannot trust that a payment succeeded just because the user landed on success_url.
5. Stripe triggers a webhook to your backend

   Stripe sends a POST request to your backend webhook endpoint.

   This includes the official record of payment status (e.g., payment_intent.succeeded, checkout.session.completed).

✅ Your backend verifies the webhook signature and processes:

    Mark the order as paid in DB

    Send email confirmation

    Allocate inventory or grant access

6. You respond to the webhook

   Your webhook handler returns HTTP 200 to Stripe.

   This ensures Stripe won’t retry sending the event.

Local : Listen to Stripe webhooks using the Stripe CLI
```
stripe listen --forward-to http://localhost:5269/api/payment/webhook-endpoint
```

</details>

TODO:

- Only ALLOW users with ADMIN Role to create and update location
