using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWallet.Application.DTOs;
using TestWallet.Application.Interfaces;
using TestWallet.Domain.Interfaces;

namespace TestWallet.Application.Services
{
    public class BalanceManagementService : IBalanceManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BalanceManagementService> _logger;

        public BalanceManagementService(IUserRepository userRepository, ILogger<BalanceManagementService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BalanceDto> GetBalanceAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            return new BalanceDto(user.Id, user.Balance);
        }

        public async Task<BalanceDto> DepositAsync(Guid userId, decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogWarning("Deposit attempt with non-positive amount: {Amount}", amount);
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            user.Balance += amount;
            await _userRepository.UpdateAsync(user);

            return new BalanceDto(user.Id, user.Balance);
        }

        public async Task<BalanceDto> WithdrawAsync(Guid userId, decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogWarning("Attempted withdrawal with non-positive amount: {Amount}", amount);
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            if (user.Balance < amount) throw new InvalidOperationException("Withdrawal amount exceeds the balance amount");

            user.Balance -= amount;
            await _userRepository.UpdateAsync(user);

            return new BalanceDto(user.Id, user.Balance);
        }
    }

}
