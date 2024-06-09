using Application.Mapper;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add SwaggerGen and configure JWT Bearer authentication
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Configure DbContext
            builder.Services.AddDbContext<CompanyContext>(options =>
            options.UseInMemoryDatabase("CompanyDb"));

            // Register Repositories
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAddressReposiotry, AddressRepository>();

            // Register Services
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<AddressService>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register Password Hasher
            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApi v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<Middlewares.CustomHeaderMiddleware>();

            app.MapControllers();

            // Ensure database is created and seeded
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<CompanyContext>();
                dbContext.Database.EnsureDeleted();  // Clear the database before seeding
                dbContext.Database.EnsureCreated();
                SeedDatabase(services);
            }

            app.Run();
        }

        private static void SeedDatabase(IServiceProvider services)
        {
            var context = services.GetRequiredService<CompanyContext>();

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

            context.Accounts.AddRange(account1, account2);

            context.Addresses.AddRange(
                new Address
                {
                    Id = address1Id,
                    City = "Warszawa",
                    Street = "Krakowskie Przedmieœcie",
                    HouseNumber = "1",
                    PostalCode = "00-001",
                    Country = "Polska"
                },
                new Address
                {
                    Id = address2Id,
                    City = "Kraków",
                    Street = "Floriañska",
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
        }
    }
}
