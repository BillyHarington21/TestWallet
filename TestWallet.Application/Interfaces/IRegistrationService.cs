using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWallet.Application.DTOs;

namespace TestWallet.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<UserDto> RegisterAsync(string email);
    }
}
