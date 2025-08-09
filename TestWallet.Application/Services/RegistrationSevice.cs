using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWallet.Application.DTOs;
using TestWallet.Application.Interfaces;
using TestWallet.Domain.Entities;
using TestWallet.Domain.Interfaces;

namespace TestWallet.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(IUserRepository userRepository, ILogger<RegistrationService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDto> RegisterAsync(string email)
        {
            var exist = await _userRepository.GetByEmailAsync(email);
            if (exist != null)
                throw new InvalidOperationException("User with this email already exists");

            var user = new User { Id = Guid.NewGuid(), Email = email, Balance = 0m };
            await _userRepository.AddAsync(user); 
            _logger.LogInformation("User created {UserId}", user.Id);
            return new UserDto(user.Id, user.Email, user.Balance);
        }
    }

}
