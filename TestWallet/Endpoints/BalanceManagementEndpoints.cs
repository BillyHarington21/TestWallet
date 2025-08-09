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
                var balanceDto = await balanceService.WithdrawAsync(userId, amount);
                return Results.Ok(balanceDto);
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
