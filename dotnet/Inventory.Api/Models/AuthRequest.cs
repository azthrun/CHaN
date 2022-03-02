namespace Inventory.Api.Models;

public class AuthRequest
{
    public string? ControllerEndpoint { get; set; } = default;
    public string? Email { get; set; }
    public string? ClientSecret { get; set; }

    public static AuthRequest NewUserRequest(string secret)
        => new() { ControllerEndpoint = "User.Create", Email = "NewUser@newuser.com", ClientSecret = secret };
}
