using Application.Mapper;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using GraphQL.Roots;
using GraphQL.Types;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CompanyContext>(options =>
            options.UseInMemoryDatabase("CompanyDb"));

            // Register repositories
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAddressReposiotry, AddressRepository>();

            // Register services
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<AddressService>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register password hasher
            builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            // Register GraphQL services
            builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddType<EmployeeType>()
                .AddType<AccountType>()
                .AddType<AddressType>()
                .AddType<GenderType>()
                .AddProjections()
                .AddFiltering()
                .AddSorting();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.MapGraphQL();

            app.Run();
        }
    }
}
