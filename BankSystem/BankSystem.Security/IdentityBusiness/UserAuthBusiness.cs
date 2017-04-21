using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.DAL.DomainModel;
using Microsoft.AspNetCore.Identity;
using BankSystem.Security.Models;
using Microsoft.AspNetCore.Http.Authentication;
using BankSystem.Security.Helpers;

namespace BankSystem.Security.IdentityBusiness
{
    public class UserAuthBusiness : IUserAuthBusiness
    {
        public ApplicationSignInManager SignInManager { get; set; }
        public ApplicationUserManager UserManager { get; set; }

        public UserAuthBusiness(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public Task<IdentityResult> CreateAsync(UserInformation user, string password = null)
        {
            var userAuth = user.MapFromUserInfo();
            Task<IdentityResult> result;
            if (string.IsNullOrEmpty(password))
            {
                result = UserManager.CreateAsync(userAuth, password);
            }
            else
            {
                result = UserManager.CreateAsync(userAuth);
            }

            user.Id = userAuth.Id;
            return result;
        }

        public Task SignInAsync(UserInformation user, bool isPersistent)
        {
            var taskResult = SignInManager.SignInAsync(user.MapFromUserInfo(), isPersistent);

            return taskResult;
        }

        public Task<IdentityResult> ConfirmEmailAsync(UserInformation userInfo, string token)
        {
            var taskResult = UserManager.ConfirmEmailAsync(userInfo.MapFromUserInfo(), token);
            return taskResult;
        }

        public  UserInformation FindByNameAsync(string userName)
        {
            var taskResult =  UserManager.FindByNameAsync(userName).Result;
            return taskResult.MapFromUserInfo();
        }

        public Task<bool> IsEmailConfirmedAsync(UserInformation userInfo)
        {

            var taskResult = UserManager.IsEmailConfirmedAsync(userInfo.MapFromUserInfo());
            return taskResult;
        }

        public Task<IdentityResult> ResetPasswordAsync(UserInformation userInfo, string token, string newPassword)
        {
            var taskResult = UserManager.ResetPasswordAsync(userInfo.MapFromUserInfo(), token, newPassword);
            return taskResult;
        }

        public Task<IList<string>> GetValidTwoFactorProvidersAsync(UserInformation userInfo)
        {
            var taskResult = UserManager.GetValidTwoFactorProvidersAsync(userInfo.MapFromUserInfo());
            return taskResult;
        }

        public Task<SignInResult> ExternalSignInAsync(string loginProvier, string provider, bool isPersistent)
        {
            var taskResult = SignInManager.ExternalLoginSignInAsync(loginProvier, provider, isPersistent);
            return taskResult;
        }

        public Task<IdentityResult> AddLoginAsync(UserInformation userInfo, UserLoginInfo userLoginInfo)
        {
            var taskResult = UserManager.AddLoginAsync(userInfo.MapFromUserInfo(), userLoginInfo);
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
            return task.Result.MapFromUserInfo();
        }

        public Task<string> GenerateTwoFactorTokenAsync(UserInformation userInfo, string tokenProvider)
        {
           
            return UserManager.GenerateTwoFactorTokenAsync(userInfo.MapFromUserInfo(), tokenProvider);
        }

        public UserInformation FindByEmailAsync(string email)
        {
            return UserManager.FindByEmailAsync(email).Result.MapFromUserInfo();
        }

        public UserInformation FindByIdAsync(string userId)
        {
            return UserManager.FindByIdAsync(userId).Result.MapFromUserInfo();
        }

        public Task<string> GetPhoneNumberAsync(UserInformation user)
        {
            return UserManager.GetPhoneNumberAsync(user.MapFromUserInfo());
        }

        public Task<string> GetEmailAsync(UserInformation user)
        {
            return UserManager.GetEmailAsync(user.MapFromUserInfo());
        }
    }
}
