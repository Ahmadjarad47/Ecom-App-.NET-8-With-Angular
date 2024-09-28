using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public record RegisterDTO : LoginDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

    }
    public record RestPasswordDTO : LoginDTO
    {
        public string Token { get; set; }
    }
    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
