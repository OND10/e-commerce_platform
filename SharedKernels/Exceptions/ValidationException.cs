using SharedKernels.Results;

namespace SharedKernels.Exceptions;

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : DomainException
{
    public IReadOnlyCollection<Error> Errors { get; }

    public ValidationException(IReadOnlyCollection<Error> errors) 
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(Error error) 
        : base(error.Description)
    {
        Errors = new[] { error };
    }

    public ValidationException(string message) 
        : base(message)
    {
        Errors = new[] { Error.Validation("Validation", message) };
    }
}
