using Application.Dtos;
using Application.Services;
using Domain.Entities;
using GraphQL.Mutations;

namespace GraphQL.Roots
{
    public class Mutation
    {
        public EmployeeMutation Employee => new EmployeeMutation();
        public AccountMutation Account => new AccountMutation();
        public AddressMutation Address => new AddressMutation();
    }
}
