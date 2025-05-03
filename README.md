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