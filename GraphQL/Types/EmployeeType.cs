using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class EmployeeType : ObjectType<Employee>
    {
        protected override void Configure(IObjectTypeDescriptor<Employee> descriptor)
        {
            descriptor.Field(e => e.Id).Type<NonNullType<IdType>>();
            descriptor.Field(e => e.Name).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Surname).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.BirthDate).Type<NonNullType<DateType>>();
            descriptor.Field(e => e.Pesel).Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Gender).Type<NonNullType<GenderType>>();

            descriptor.Field(e => e.AddressId).Type<IdType>();
            descriptor.Field(e => e.Address).Type<AddressType>()
                .ResolveWith<EmployeeResolver>(e => e.GetAddress(default!, default!))
                .UseDbContext<CompanyContext>();

            descriptor.Field(e => e.AccountId).Type<IdType>();
            descriptor.Field(e => e.Account).Type<AccountType>()
                .ResolveWith<EmployeeResolver>(e => e.GetAccount(default!, default!))
                .UseDbContext<CompanyContext>();
        }

        private class EmployeeResolver
        {
            public async Task<Address> GetAddress([Parent] Employee employee, [Service] CompanyContext context)
            {
                var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == employee.AddressId);
                if(address is null)
                    throw new KeyNotFoundException($"Address with id: {employee.AddressId} not found");
                return address;
            }

            public async Task<Account> GetAccount([Parent] Employee employee, [Service] CompanyContext context)
            {
                var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == employee.AccountId);
                if(account is null)
                    throw new KeyNotFoundException($"Account with id: {employee.AccountId} not found");
                return account;
            }
        }
    }
}
