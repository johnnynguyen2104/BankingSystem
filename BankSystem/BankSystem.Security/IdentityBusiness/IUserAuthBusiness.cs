using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;

namespace BankSystem.Security.IdentityBusiness
{
    public interface IUserAuthBusiness
    {
        string GetUserName(ClaimsPrincipal claim);

        bool IsSignedIn(ClaimsPrincipal claim);

        Task<string> GetPhoneNumberAsync(UserInformation user);

        Task<string> GetEmailAsync(UserInformation user);

        UserInformation FindByEmailAsync(string email);

        UserInformation FindByIdAsync(string userId);

        Task<string> GenerateTwoFactorTokenAsync(UserInformation userInfo, string tokenProvider);

        UserInformation GetTwoFactorAuthenticationUserAsync();

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberBrowser);

        Task<IdentityResult> CreateAsync(UserInformation user, string password = null);

        Task SignInAsync(UserInformation user, bool isPersistent);

        Task<IdentityResult> ConfirmEmailAsync(UserInformation userInfo, string token);

        UserInformation FindByNameAsync(string userName);

        Task<bool> IsEmailConfirmedAsync(UserInformation user);

        Task<IdentityResult> ResetPasswordAsync(UserInformation user, string token, string newPassword);

        Task<IList<string>> GetValidTwoFactorProvidersAsync(UserInformation user);

        Task<SignInResult> ExternalSignInAsync(string loginProvier, string provider, bool isPersistent);

        Task<IdentityResult> AddLoginAsync(UserInformation user, UserLoginInfo userLoginInfo);

        Task SignOutAsync();

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);

        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectXsrf = null);

        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
    }
}
