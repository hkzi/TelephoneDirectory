using FluentValidation.Results;

namespace TelephoneDirectory.Core.Extensions;

public class ValidationErrorDetails:ErrorDetails
{
    public IEnumerable<ValidationFailure> Errors { get; set; }
}