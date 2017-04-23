using Microsoft.Extensions.DependencyInjection;
using System;
using BankSystem.DAL.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BankSystem.Security
{
    public class IdentityModule
    {
        public static void RegisterIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options => {
                options.Cookies.ApplicationCookie.LoginPath = "/User/Login";
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddSignInManager<ApplicationSignInManager>()
                .AddUserManager<ApplicationUserManager>()
                .AddEntityFrameworkStores<BankSystemDbContext>()
                .AddDefaultTokenProviders();

            //add DI 
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }

    public class ApplicationSignInManager : SignInManager<User>
    {
        public ApplicationSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
        }
    }
}
