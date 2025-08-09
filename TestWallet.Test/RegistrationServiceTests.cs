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
    public class RegistrationServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenEmailIsNew()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<RegistrationService>>();

            var service = new RegistrationService(userRepoMock.Object, loggerMock.Object);

            var email = "test@example.com";

            var userDto = await service.RegisterAsync(email);

            Assert.NotNull(userDto);
            Assert.Equal(email, userDto.Email);
            Assert.Equal(0, userDto.Balance);

            userRepoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
            userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
        {
            var existingUser = new User { Id = Guid.NewGuid(), Email = "exist@example.com", Balance = 0 };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(existingUser);

            var loggerMock = new Mock<ILogger<RegistrationService>>();

            var service = new RegistrationService(userRepoMock.Object, loggerMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync("exist@example.com"));

            userRepoMock.Verify(r => r.GetByEmailAsync("exist@example.com"), Times.Once);
            userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
