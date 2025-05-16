namespace TicketService.Domain.Exceptions;

public class TicketAlreadyCancelledException(string message) : Exception(message);
