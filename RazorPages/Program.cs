using Application.Mapper;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                options.UseInMemoryDatabase("CompanyDb"));

            // Add Identity services
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CompanyContext>();

            // Configure cookie authentication
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
            });

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

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
                options.AddPolicy("AdminOrUserPolicy", policy => policy.RequireRole("Admin", "User"));
            });

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
            var context = services.GetRequiredService<CompanyContext>();

            // Seed roles
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Created role: {role}");
                }
            }

            // Seed users
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

            // Seed data with static GUIDs
            var address1Id = new Guid("8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");
            var address2Id = new Guid("9d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");
            var account1Id = new Guid("1d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");
            var account2Id = new Guid("2d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");
            var employee1Id = new Guid("3d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");
            var employee2Id = new Guid("4d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a");

            var account1 = new Account
            {
                Id = account1Id,
                Username = "admin",
                Email = "admin@example.com",
                IsActive = true
            };
            account1.Password = new PasswordHasher<Account>().HashPassword(account1, "admin");

            var account2 = new Account
            {
                Id = account2Id,
                Username = "user",
                Email = "user@example.com",
                IsActive = true
            };
            account2.Password = new PasswordHasher<Account>().HashPassword(account2, "user");

            if (!context.Accounts.Any(a => a.Id == account1Id))
            {
                context.Accounts.AddRange(account1, account2);

                context.Addresses.AddRange(
                    new Address
                    {
                        Id = address1Id,
                        City = "Warszawa",
                        Street = "Krakowskie Przedmieście",
                        HouseNumber = "1",
                        PostalCode = "00-001",
                        Country = "Polska"
                    },
                    new Address
                    {
                        Id = address2Id,
                        City = "Kraków",
                        Street = "Floriańska",
                        HouseNumber = "2",
                        PostalCode = "31-021",
                        Country = "Polska"
                    }
                );

                context.Employees.AddRange(
                    new Employee
                    {
                        Id = employee1Id,
                        Name = "Jan",
                        Surname = "Kowalski",
                        BirthDate = new DateTime(1980, 5, 15),
                        Pesel = "80051512345",
                        Gender = Domain.Enums.Gender.Male,
                        AddressId = address1Id,
                        AccountId = account1Id
                    },
                    new Employee
                    {
                        Id = employee2Id,
                        Name = "Anna",
                        Surname = "Nowak",
                        BirthDate = new DateTime(1990, 7, 20),
                        Pesel = "90072012345",
                        Gender = Domain.Enums.Gender.Female,
                        AddressId = address2Id,
                        AccountId = account2Id
                    }
                );

                context.SaveChanges();
                logger.LogInformation("Seeded the database with accounts, addresses, and employees");
            }
            else
            {
                logger.LogInformation("Database already contains seeded data");
            }
        }

    }
}
