using Inventory.Api.Abstractions;
using Inventory.Api.Data;
using Inventory.Api.Models;
using Inventory.Api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Inventory.Api.Authentication;

public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration configuration;
    private readonly IRepository<User> repository;

    public AuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration,
        CosmosContainerProvider cosmosContainerProvider
    ) : base(options, logger, encoder, clock)
    {
        this.configuration = configuration;
        this.repository = new UserRepository(cosmosContainerProvider);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(authHeader.Parameter!);
            var authRequestJson = System.Text.Encoding.UTF8.GetString(bytes);
            var authRequest = JsonConvert.DeserializeObject<AuthRequest>(authRequestJson);

            if (authRequest is null || string.IsNullOrEmpty(authRequest.ClientSecret))
                return AuthenticateResult.Fail("Invalid authentication request");

            if (authRequest.ControllerEndpoint != "User.Create")
            {
                if (string.IsNullOrEmpty(authRequest.Email)) return AuthenticateResult.Fail("Invalid authentication request");
                _ = await repository.ReadAsync(authRequest.Email);
            }

            if (authRequest.ClientSecret == configuration["ClientSecret"])
            {
                var claimName = string.IsNullOrEmpty(authRequest.Email) ? "NewUser" : authRequest.Email;
                var claims = new[] { new Claim(ClaimTypes.Name, claimName) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Invalid credentials");
            }
        }
        catch (Inventory.Api.Exceptions.NotFoundException)
        {
            return AuthenticateResult.Fail("Invalid user");
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Error: {ex.Message}");
        }
    }
}