using System.ComponentModel.DataAnnotations;

namespace TicketService.API.Validations;

public class NotEmptyGuidAttribute : ValidationAttribute
{
	override protected ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is Guid guid && guid != Guid.Empty)
		{
			return ValidationResult.Success;
		}

		return new ValidationResult(ErrorMessage ?? "Guid cannot be empty.");
	}
}