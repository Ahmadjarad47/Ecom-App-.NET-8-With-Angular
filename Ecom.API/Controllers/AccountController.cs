using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-address-for-user")]
        public async Task<IActionResult> getAddress()
        {
            var address = await work.Auth.getUserAddress(User.FindFirst(ClaimTypes.Email).Value);
            var result = mapper.Map<ShipAddressDTO>(address);
            return Ok(result);
        }

        [HttpGet("Logout")]
        public void logout()
        {

            Response.Cookies.Append("token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(-1)
            });
        }

        [Authorize]
        [HttpGet("get-user-name")]
        public IActionResult GetUserName()
        {
            return Ok(new ResponseAPI(200, User.Identity.Name));
        }


        [HttpGet("IsUserAuth")]
        public async Task<IActionResult> IsUserAuth()
        {

            return User.Identity.IsAuthenticated ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("update-address")]
        public async Task<IActionResult> updateAddress(ShipAddressDTO addressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = mapper.Map<Address>(addressDTO);
            var result = await work.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDTO>> register(RegisterDTO registerDTO)
        {
            string result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "done")
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            return Ok(new ResponseAPI(200, result));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTO loginDTO)
        {


            string result = await work.Auth.LoginAsync(loginDTO);
            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1)
            });
            return Ok(new ResponseAPI(200));
        }





        [HttpPost("active-account")]
        public async Task<ActionResult<ActiveAccountDTO>> active(ActiveAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }








        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(200));
        }






        [HttpPost("reset-password")]
        public async Task<IActionResult> reset(RestPasswordDTO restPasswordDTO)
        {
            var result = await work.Auth.ResetPassword(restPasswordDTO);
            if (result == "done")
            {
                return Ok(new ResponseAPI(200));
            }
            return BadRequest(new ResponseAPI(400));
        }
    }
}
