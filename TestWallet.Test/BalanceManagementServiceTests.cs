using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWallet.Application.Services;
using TestWallet.Domain.Entities;
using TestWallet.Domain.Interfaces;

namespace TestWallet.Test
{
    public class BalanceManagementServiceTests
    {
        [Fact]
        public async Task DepositAsync_ShouldAddAmount_WhenAmountIsPositive()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Balance = 10 };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<BalanceManagementService>>();

            var service = new BalanceManagementService(userRepoMock.Object, loggerMock.Object);

            var amount = 5m;

            var balanceDto = await service.DepositAsync(user.Id, amount);

            Assert.Equal(user.Id, balanceDto.UserId);
            Assert.Equal(15m, balanceDto.Balance);

            userRepoMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
            userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task DepositAsync_ShouldThrow_WhenAmountNotPositive()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<BalanceManagementService>>();
            var service = new BalanceManagementService(userRepoMock.Object, loggerMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.DepositAsync(Guid.NewGuid(), 0));
            await Assert.ThrowsAsync<ArgumentException>(() => service.DepositAsync(Guid.NewGuid(), -5));
        }

        [Fact]
        public async Task WithdrawAsync_ShouldSubtractAmount_WhenSufficientBalance()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Balance = 20 };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            userRepoMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<BalanceManagementService>>();

            var service = new BalanceManagementService(userRepoMock.Object, loggerMock.Object);

            var amount = 10m;

            var balanceDto = await service.WithdrawAsync(user.Id, amount);

            Assert.Equal(user.Id, balanceDto.UserId);
            Assert.Equal(10m, balanceDto.Balance);

            userRepoMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
            userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task WithdrawAsync_ShouldThrow_WhenInsufficientFunds()
        {
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Balance = 5 };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            var loggerMock = new Mock<ILogger<BalanceManagementService>>();

            var service = new BalanceManagementService(userRepoMock.Object, loggerMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.WithdrawAsync(user.Id, 10));
        }
    }
}
