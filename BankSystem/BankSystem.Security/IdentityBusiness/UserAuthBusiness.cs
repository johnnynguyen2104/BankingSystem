using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.DAL.DomainModels;
using Microsoft.AspNetCore.Identity;
using BankSystem.Security.Models;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace BankSystem.Security.IdentityBusiness
{
    public class UserAuthBusiness : IUserAuthBusiness
    {
        private ApplicationSignInManager SignInManager { get; set; }
        private ApplicationUserManager UserManager { get; set; }

        private readonly IMapper Mapper;

        public UserAuthBusiness(ApplicationSignInManager signInManager, ApplicationUserManager userManager, IMapper mapper)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Mapper = mapper;
        }

        public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            
            var taskResult = SignInManager.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
            return taskResult;
        }

        public Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            var taskResult = SignInManager.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser);
            return taskResult;
        }

        public async Task<IdentityResult> CreateAsync(UserInformation user, string password = null)
        {
            var userAuth = Mapper.Map<User>(user);
            IdentityResult result;

            //Internal register
            if (!string.IsNullOrEmpty(password))
            {
                result = await UserManager.CreateAsync(userAuth, password);
            }
            else
            {
                //external register
                result = await UserManager.CreateAsync(userAuth);
            }

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(userAuth, false);
            }

            return result;
        }

        public Task SignInAsync(UserInformation user, bool isPersistent)
        {
            var taskResult = SignInManager.SignInAsync(Mapper.Map<User>(user), isPersistent);
          
            return taskResult;
        }

        public Task<IdentityResult> ConfirmEmailAsync(UserInformation userInfo, string token)
        {
            var taskResult = UserManager.ConfirmEmailAsync(Mapper.Map<User>(userInfo), token);
            return taskResult;
        }

        public  UserInformation FindByNameAsync(string userName)
        {
            var taskResult =  UserManager.FindByNameAsync(userName).Result;
            return Mapper.Map<UserInformation>(taskResult);
        }

        public Task<bool> IsEmailConfirmedAsync(UserInformation userInfo)
        {

            var taskResult = UserManager.IsEmailConfirmedAsync(Mapper.Map<User>(userInfo));
            return taskResult;
        }

        public Task<IdentityResult> ResetPasswordAsync(UserInformation userInfo, string token, string newPassword)
        {
            var taskResult = UserManager.ResetPasswordAsync(Mapper.Map<User>(userInfo), token, newPassword);
            return taskResult;
        }

        public Task<IList<string>> GetValidTwoFactorProvidersAsync(UserInformation userInfo)
        {
            var taskResult = UserManager.GetValidTwoFactorProvidersAsync(Mapper.Map<User>(userInfo));
            return taskResult;
        }

        public Task<SignInResult> ExternalSignInAsync(string loginProvier, string provider, bool isPersistent)
        {
            var taskResult = SignInManager.ExternalLoginSignInAsync(loginProvier, provider, isPersistent);
            return taskResult;
        }

        public Task<IdentityResult> AddLoginAsync(UserInformation userInfo, UserLoginInfo userLoginInfo)
        {
            var taskResult = UserManager.AddLoginAsync(Mapper.Map<User>(userInfo), userLoginInfo);
            return taskResult;
        }

        public Task SignOutAsync()
        {
            return SignInManager.SignOutAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectXsrf = null)
        {
            return SignInManager.GetExternalLoginInfoAsync(expectXsrf);
        }

        public Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return SignInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public UserInformation GetTwoFactorAuthenticationUserAsync()
        {
            
            var task = SignInManager.GetTwoFactorAuthenticationUserAsync();

            return task == null ? null : Mapper.Map<UserInformation>(task);
        }

        public Task<string> GenerateTwoFactorTokenAsync(UserInformation userInfo, string tokenProvider)
        {
           
            return UserManager.GenerateTwoFactorTokenAsync(Mapper.Map<User>(userInfo), tokenProvider);
        }

        public UserInformation FindByEmailAsync(string email)
        {
            var result = UserManager.FindByEmailAsync(email).Result;
         
            return result == null ? null : Mapper.Map<UserInformation>(result);
        }

        public UserInformation FindByIdAsync(string userId)
        {
            var result = UserManager.FindByIdAsync(userId).Result;

            return result == null ? null : Mapper.Map<UserInformation>(result);
        }

        public Task<string> GetPhoneNumberAsync(UserInformation user)
        {
            return UserManager.GetPhoneNumberAsync(Mapper.Map<User>(user));
        }

        public Task<string> GetEmailAsync(UserInformation user)
        {
            return UserManager.GetEmailAsync(Mapper.Map<User>(user));
        }

        public bool IsSignedIn(ClaimsPrincipal claim)
        {
            return SignInManager.IsSignedIn(claim);
        }

        public string GetUserName(ClaimsPrincipal claim)
        {
            return UserManager.GetUserName(claim);
        }

        public Task<IdentityResult> AddClaims(ClaimsPrincipal claimPrincipal, IList<Claim> claim)
        {
            var user = UserManager.GetUserAsync(claimPrincipal).Result;
            return UserManager.AddClaimsAsync(user, claim);
        }

        public Task<IdentityResult> RemoveClaims(ClaimsPrincipal claimPrincipal, IList<Claim> claim)
        {
            var user = UserManager.GetUserAsync(claimPrincipal).Result;
            return UserManager.RemoveClaimsAsync(user, claim);
        }
    }
}
