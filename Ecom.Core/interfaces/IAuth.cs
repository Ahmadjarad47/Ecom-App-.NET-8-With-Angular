using Ecom.Core.DTO;
using Ecom.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO login);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<string> ResetPassword(RestPasswordDTO restPassword);
        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);
        Task<bool> UpdateAddress(string email, Address address);
        Task<Address> getUserAddress(string email);
    }
}
