using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyContext _context;

        public EmployeeRepository(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task<Employee> GetByIdAsync(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Address)
                .Include(e => e.Account)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(employee is null) throw new KeyNotFoundException("Employee not found");
            return employee;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee is not null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Employee not found");
            }
        }

        public async Task<Address> GetAddressAsync(Guid employeeId)
        {
            var address = await _context.Employees.Include(e => e.Address).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (address is not null)
            {
                return address.Address;
            }
            else
            {
                throw new KeyNotFoundException("Employee not found");
            }
        }

        public async Task<Address> UpdateAddressAsync(Guid employeeId, Address address)
        {
            var employee = await _context.Employees.Include(e => e.Address).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            if (employee.Address is null)
            {
                employee.Address = address;
                _context.Addresses.Add(address);
            }
            else
            {
                _context.Entry(employee.Address).CurrentValues.SetValues(address);
            }

            await _context.SaveChangesAsync();
            return employee.Address;
        }

        public async Task<Account> GetAccountAsync(Guid employeeId)
        {
            var employee = await _context.Employees.Include(e => e.Account).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee is not null)
            {
                return employee.Account;
            }
            else
            {
                throw new KeyNotFoundException("Employee not found");
            }
        }

        public async Task<Account> UpdateAccountAsync(Guid employeeId, Account account)
        {
            var employee = await _context.Employees.Include(e => e.Account).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee is null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            if (employee.Account is null)
            {
                employee.Account = account;
                _context.Accounts.Add(account);
            }
            else
            {
                _context.Entry(employee.Account).CurrentValues.SetValues(account);
            }

            await _context.SaveChangesAsync();
            return employee.Account;
        }
    }
}
