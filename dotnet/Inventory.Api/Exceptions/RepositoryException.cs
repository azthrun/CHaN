namespace Inventory.Api.Exceptions;

public class RepositoryException : HttpException
{
    public RepositoryException() : base()
    {
    }

    public RepositoryException(string message) : base(message)
    {
    }

    public RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public RepositoryException(int statusCode, object? payload = null) : base(statusCode, payload)
    {
    }
}