using Application.Mapper;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RazorPages
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Configure DbContext
            builder.Services.AddDbContext<CompanyContext>(options =>
                options.UseInMemoryDatabase("ApplicationDb"));

            // Add Identity services
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register Password Hasher
            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            // Add Repositories and Services
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAddressReposiotry, AddressRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<EmployeeService>();

            // Add logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedDatabase(services, logger, userManager, roleManager);
            }

            app.Run();
        }

        private static async Task SeedDatabase(IServiceProvider services, ILogger<Program> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Created role: {role}");
                }
            }

            var adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true };
            var regularUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com", EmailConfirmed = true };

            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Created admin user with password 'Admin@123'");
                }
                else
                {
                    logger.LogError("Failed to create admin user");
                }
            }

            if (userManager.Users.All(u => u.UserName != regularUser.UserName))
            {
                var result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                    logger.LogInformation("Created regular user with password 'User@123'");
                }
                else
                {
                    logger.LogError("Failed to create regular user");
                }
            }
        }

        private interface IRoleRepository
        {
        }
    }
}
