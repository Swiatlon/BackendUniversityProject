using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace GraphQL.Types
{
    public class AccountType : ObjectType<Account>
    {
        protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
        {
            descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.Username).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.Password).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.Email).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.IsActive).Type<NonNullType<BooleanType>>();
            descriptor.Field(a => a.DeactivationDate).Type<DateType>();

            descriptor.Field(a => a.EmployeeId).Type<IdType>();
            descriptor.Field(a => a.Employee).Type<EmployeeType>()
                .ResolveWith<AccountResolver>(a => a.GetEmployee(default!, default!))
                .UseDbContext<CompanyContext>();
        }
        private class AccountResolver
        {
            public async Task<Employee> GetEmployee([Parent] Account account, [Service] CompanyContext context)
            {
                var employee = await context.Employees.FirstOrDefaultAsync(a => a.Id == account.Id);
                if(employee is null)
                    throw new KeyNotFoundException($"Employee with id: {account.Id} not found");
                return employee;
            }
        }
    }
}
