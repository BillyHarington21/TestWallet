using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWallet.Application.DTOs;

namespace TestWallet.Application.Interfaces
{
    public interface IBalanceManagementService
    {
        Task<BalanceDto> GetBalanceAsync(Guid userId);
        Task<BalanceDto> DepositAsync(Guid userId, decimal amount);
        Task<BalanceDto> WithdrawAsync(Guid userId, decimal amount);
    }
}
