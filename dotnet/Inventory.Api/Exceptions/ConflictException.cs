namespace Inventory.Api.Exceptions;

public class ConflictException : HttpException
{
    public ConflictException(object? entity) : base(StatusCodes.Status409Conflict, entity)
    {
    }
}
