using Application.Dtos;
using Application.Services;

namespace GraphQL.Mutations
{
    public class EmployeeMutation
    {
        public async Task<EmployeeFullDto> AddEmployee(EmployeeFullDto employeeDto, [Service] EmployeeService employeeService)
        {
            return await employeeService.AddEmployeeAsync(employeeDto);
        }

        public async Task<EmployeeFullDto> UpdateEmployee(EmployeeFullDto employeeDto, [Service] EmployeeService employeeService)
        {
            return await employeeService.UpdateEmployeeAsync(employeeDto);
        }

        public async Task<bool> DeleteEmployee(Guid id, [Service] EmployeeService employeeService)
        {
            await employeeService.DeleteEmployeeAsync(id);
            return true;
        }

        public async Task<AddressOnlyDto> UpdateEmployeeAddress(Guid id, AddressOnlyDto addressDto, [Service] EmployeeService employeeService)
        {
            return await employeeService.UpdateEmployeeAddressAsync(id, addressDto);
        }

        public async Task<AccountOnlyDto> UpdateEmployeeAccount(Guid id, AccountOnlyDto accountDto, [Service] EmployeeService employeeService)
        {
            return await employeeService.UpdateEmployeeAccountAsync(id, accountDto);
        }
    }
}
