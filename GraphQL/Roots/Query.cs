using Application.Dtos;
using Application.Services;
using GraphQL.Queries;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQL.Roots
{
    public class Query
    {
        public EmployeeQuery Employee => new EmployeeQuery();
        public AccountQuery Account => new AccountQuery();
        public AddressQuery Address => new AddressQuery();
        
    }
}
