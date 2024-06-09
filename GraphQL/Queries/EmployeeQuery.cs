using Application.Dtos;
using Application.Services;

namespace GraphQL.Queries
{
    public class EmployeeQuery
    {
        public async Task<IEnumerable<EmployeeOnlyDto>> GetEmployees([Service] EmployeeService employeeService)
        {
            return await employeeService.GetAllEmployeesAsync();
        }

        public async Task<EmployeeFullDto> GetEmployeeById(Guid id, [Service] EmployeeService employeeService)
        {
            return await employeeService.GetEmployeeByIdAsync(id);
        }
        public async Task<AddressOnlyDto> GetEmployeeAddress(Guid id, [Service] EmployeeService employeeService)
        {
            return await employeeService.GetEmployeeAddressAsync(id);
        }

        public async Task<AccountOnlyDto> GetEmployeeAccount(Guid id, [Service] EmployeeService employeeService)
        {
            return await employeeService.GetEmployeeAccountAsync(id);
        }
    }
}
