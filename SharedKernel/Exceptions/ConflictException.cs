namespace SharedKernel.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs (e.g., duplicate entity)
/// </summary>
public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
