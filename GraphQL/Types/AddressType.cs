using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class AddressType : ObjectType<Address>
    {
        protected override void Configure(IObjectTypeDescriptor<Address> descriptor)
        {
            descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
            descriptor.Field(a => a.City).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.Street).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.HouseNumber).Type<NonNullType<StringType>>();
            descriptor.Field(a => a.ApartmentNumber).Type<StringType>();
            descriptor.Field(a => a.PostalCode).Type<NonNullType<StringType>>();

            descriptor.Field(a => a.EmployeeId).Type<IdType>();
            descriptor.Field(a => a.Employee).Type<EmployeeType>()
                .ResolveWith<AddressResolver>(a => a.GetEmployee(default!, default!))
                .UseDbContext<CompanyContext>();
        }

        private class AddressResolver
        {
            public async Task<Employee> GetEmployee([Parent] Address address, [Service] CompanyContext context)
            {
                var employee = await context.Employees.FirstOrDefaultAsync(a => a.Id == address.Id);
                if(employee is null)
                    throw new KeyNotFoundException($"Employee with id: {address.Id} not found");
                return employee;
            }
        }
    }
}
