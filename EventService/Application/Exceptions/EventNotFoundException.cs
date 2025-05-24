namespace EventService.Application.Exceptions;

public class EventNotFoundException(string message) : Exception(message);
