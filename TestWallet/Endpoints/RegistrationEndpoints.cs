using TestWallet.Application.Interfaces;

namespace TestWallet.API.Endpoints
{
    public static class RegistrationEndpoints
    {
        public static void MapRegistrationEndpoints(this WebApplication app)
        {
            app.MapPost("/register", async (string email, IRegistrationService registrationService) =>
            {
                var userDto = await registrationService.RegisterAsync(email);
                return Results.Ok(userDto);
            })
            .WithName("RegisterUser")
            .WithOpenApi();
        }
    }
}
