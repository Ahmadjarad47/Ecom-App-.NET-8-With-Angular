using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class AuthRepositry : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;
        public AuthRepositry(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this UserName is already registerd";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "this email is already registerd";
            }

            AppUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DispalyName = registerDTO.DisplayName,
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            // Send Active Email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");
            return "done";
        }


        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email,
                "a.j47sharp@gmail.com",
                subject
                , EmailStringBody.send(email, code, component, message));
            await emailService.SendEmail(result);
        }




        public async Task<string> LoginAsync(LoginDTO login)
        {
            if (login == null)
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(login.Email);

            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);

                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");

                return "Please confirem your email first, we have send activat to your E-mail";
            }

            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);

            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }

            return "please check your email and password, something went wrong";
        }





        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser is null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "Rest pssword", "click on button to Reset your password");

            return true;

        }

        public async Task<string> ResetPassword(RestPasswordDTO restPassword)
        {
            var findUser = await userManager.FindByEmailAsync(restPassword.Email);
            if (findUser is null)
            {
                return null;
            }

            var result = await userManager.ResetPasswordAsync(findUser, restPassword.Token, restPassword.Password);

            if (result.Succeeded)
            {
                return "done";
            }
            return result.Errors.ToList()[0].Description;
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser is null)
            {
                return false;
            }

            var reslt = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (reslt.Succeeded)
                return true;

            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please active your email, click on button to active");

            return false;
        }
    }
}
