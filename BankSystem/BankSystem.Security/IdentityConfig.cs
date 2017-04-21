﻿using Microsoft.Extensions.DependencyInjection;
using System;
using BankSystem.DAL.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using BankSystem.Security.IdentityBusiness;

namespace BankSystem.Security
{
    public class IdentityConfig
    {
        public static void RegisterIdentity(IServiceCollection services, string connectionString)
        {
            // Add framework services.
            services.AddDbContext<BankSystemDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
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