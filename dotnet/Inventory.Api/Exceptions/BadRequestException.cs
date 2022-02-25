namespace Inventory.Api.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException() : base(StatusCodes.Status400BadRequest)
    {
    }
}
