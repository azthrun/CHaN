using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Exceptions;

public class HttpException : Exception
{
    public HttpException() : base()
    {
    }

    public HttpException(string message) : base(message)
    {
    }

    public HttpException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public HttpException(int statusCode, object? payload = null) : base()
    {
        StatusCode = statusCode;
        Payload = payload;
    }

    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    public object? Payload { get; set; }
}