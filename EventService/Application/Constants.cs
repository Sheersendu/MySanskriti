namespace EventService.Application;

public class Constants
{
	public const string DateFormat = "dd-MM-yyyy HH:mm";
	public const int PageNumber = 1;
	public const int PageSize = 10;
	public const string redisCacheKey = "events:{city.ToLower()}:page={pageNumber}:size={pageSize}";
}