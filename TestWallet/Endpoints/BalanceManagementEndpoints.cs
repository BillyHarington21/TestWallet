using Swashbuckle.AspNetCore.Annotations;
using TestWallet.Application.DTOs;
using TestWallet.Application.Interfaces;

namespace TestWallet.API.Endpoints
{
    public static class BalanceManagementEndpoints
    {
        /// <summary>
        /// Эндпоинты для управления балансом пользователя
        /// </summary>
        public static void MapBalanceManagementEndpoints(this WebApplication app)
        {
            // Пополнение баланса
            app.MapPost("/deposit",
                /// <summary>
                /// Пополняет баланс пользователя на указанную сумму.
                /// </summary>
                /// <param name="userId">Идентификатор пользователя</param>
                /// <param name="amount">Сумма пополнения</param>
                async (Guid userId, decimal amount, IBalanceManagementService balanceService) =>
                {
                    var balanceDto = await balanceService.DepositAsync(userId, amount);
                    return Results.Ok(balanceDto);
                })
            .WithName("DepositFunds")
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Пополнение баланса",
                description: "Увеличивает баланс пользователя на указанную сумму."
            ))
            .Produces<BalanceDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            // Списание средств
            app.MapPost("/withdraw",
                /// <summary>
                /// Списывает средства с баланса пользователя.
                /// </summary>
                /// <param name="userId">Идентификатор пользователя</param>
                /// <param name="amount">Сумма списания</param>
                async (Guid userId, decimal amount, IBalanceManagementService balanceService) =>
                {
                    try
                    {
                        var balanceDto = await balanceService.WithdrawAsync(userId, amount);
                        return Results.Ok(balanceDto);
                    }
                    catch (InvalidOperationException)
                    {
                        return Results.Json(new { error = "Insufficient funds" }, statusCode: StatusCodes.Status400BadRequest);
                    }
                    catch (ArgumentException ex)
                    {
                        return Results.Json(new { error = ex.Message }, statusCode: StatusCodes.Status400BadRequest);
                    }
                    catch (Exception ex)
                    {
                        return Results.Json(new { error = ex.Message }, statusCode: StatusCodes.Status500InternalServerError);
                    }
                })
            .WithName("WithdrawFunds")
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Списание средств",
                description: "Уменьшает баланс пользователя на указанную сумму. Возвращает ошибку, если средств недостаточно."
            ))
            .Produces<BalanceDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithOpenApi();

            // Получение баланса
            app.MapGet("/balance/{userId}",
                /// <summary>
                /// Возвращает текущий баланс пользователя.
                /// </summary>
                /// <param name="userId">Идентификатор пользователя</param>
                async (Guid userId, IBalanceManagementService balanceService) =>
                {
                    var balanceDto = await balanceService.GetBalanceAsync(userId);
                    return Results.Ok(balanceDto);
                })
            .WithName("GetBalance")
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Получение баланса",
                description: "Возвращает текущий баланс пользователя по его идентификатору."
            ))
            .Produces<BalanceDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}
