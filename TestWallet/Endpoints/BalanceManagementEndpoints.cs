using TestWallet.Application.Interfaces;

namespace TestWallet.API.Endpoints
{
    public static class BalanceManagementEndpoints
    {
        public static void MapBalanceManagementEndpoints(this WebApplication app)
        {
            app.MapPost("/deposit", async (Guid userId, decimal amount, IBalanceManagementService balanceService) =>
            {
                var balanceDto = await balanceService.DepositAsync(userId, amount);
                return Results.Ok(balanceDto);
            })
            .WithName("DepositFunds")
            .WithOpenApi();

            app.MapPost("/withdraw", async (Guid userId, decimal amount, IBalanceManagementService balanceService) =>
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
            .WithOpenApi();

            app.MapGet("/balance/{userId}", async (Guid userId, IBalanceManagementService balanceService) =>
            {
                var balanceDto = await balanceService.GetBalanceAsync(userId);
                return Results.Ok(balanceDto);
            })
            .WithName("GetBalance")
            .WithOpenApi();
        }
    }
}
