using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(Guid id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);

        // Address management methods
        Task<Address> GetAddressAsync(Guid employeeId);
        Task<Address> UpdateAddressAsync(Guid employeeId, Address address);

        // Account management methods
        Task<Account> GetAccountAsync(Guid employeeId);
        Task<Account> UpdateAccountAsync(Guid employeeId, Account account);
    }
}
