using Swashbuckle.AspNetCore.Annotations;
using TestWallet.Application.DTOs;
using TestWallet.Application.Interfaces;

namespace TestWallet.API.Endpoints
{
    public static class RegistrationEndpoints
    {
        /// <summary>
        /// Эндпоинты для регистрации пользователей
        /// </summary>
        public static void MapRegistrationEndpoints(this WebApplication app)
        {
            app.MapPost("/register",
                /// <summary>
                /// Регистрирует нового пользователя по email.
                /// </summary>
                /// <param name="email">Email пользователя</param>
                /// <param name="registrationService">Сервис регистрации</param>
                async (string email, IRegistrationService registrationService) =>
                {
                    var userDto = await registrationService.RegisterAsync(email);
                    return Results.Ok(userDto);
                })
            .WithName("RegisterUser")
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Регистрация пользователя",
                description: "Создает нового пользователя по указанному email и возвращает его данные."
            ))
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();
        }
    }
}
