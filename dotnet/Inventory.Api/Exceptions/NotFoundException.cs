namespace Inventory.Api.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException() : base(StatusCodes.Status404NotFound)
    {
    }
}