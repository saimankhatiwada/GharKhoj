namespace GharKhoj.Application.Exceptions;

public sealed class UniquenessViolationException : Exception
{
    public UniquenessViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
