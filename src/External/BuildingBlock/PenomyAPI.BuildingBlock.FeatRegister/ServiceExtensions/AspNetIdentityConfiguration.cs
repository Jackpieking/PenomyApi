using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions
{
    public static class AspNetIdentityConfiguration
    {
        private const string LowerLetters = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string UpperLetters = LowerLetters.ToUpper();
        private const string Numbers = "0123456789";
        private const string SpecialCharacters = "-._@+";
        private static readonly string AllowedCharacters =
            $"{LowerLetters}{UpperLetters}{Numbers}{SpecialCharacters}";

        public static IServiceCollection AddAspNetIdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddIdentity<PgUser, PgRole>(setupAction: options =>
                {
                    // Password configuration.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 4;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout configuration.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(value: 1);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.AllowedForNewUsers = true;

                    // User's credentials configuration.
                    options.User.AllowedUserNameCharacters = $"{AllowedCharacters}";
                    options.User.RequireUniqueEmail = true;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
