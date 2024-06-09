using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeOnlyDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeFullDto>> GetEmployeeById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee is null)
                return NotFound("Employee not found");
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeFullDto>> AddEmployee(EmployeeFullDto employeeDto)
        {
            var employee = await _employeeService.AddEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, EmployeeFullDto employeeDto)
        {
            if (id != employeeDto.Id)
                return BadRequest("Employee ID mismatch");

            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDto);
            if (updatedEmployee is null)
                return NotFound("Employee not found");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/address")]
        public async Task<ActionResult<AddressOnlyDto>> GetEmployeeAddress(Guid id)
        {
            var address = await _employeeService.GetEmployeeAddressAsync(id);
            if (address is null)
                return NotFound("Address not found");
            return Ok(address);
        }

        [HttpPut("{id}/address")]
        public async Task<IActionResult> UpdateEmployeeAddress(Guid id, AddressOnlyDto addressDto)
        {
            var updatedAddress = await _employeeService.UpdateEmployeeAddressAsync(id, addressDto);
            if (updatedAddress is null)
                return NotFound("Address not found");
            return Ok(updatedAddress);
        }

        [HttpGet("{id}/accounts")]
        public async Task<ActionResult<AccountOnlyDto>> GetEmployeeAccount(Guid id)
        {
            var account = await _employeeService.GetEmployeeAccountAsync(id);
            if (account is null)
                return NotFound("Account not found");
            return Ok(account);
        }

        [HttpPut("{id}/accounts")]
        public async Task<IActionResult> UpdateEmployeeAccount(Guid id, AccountOnlyDto accountDto)
        {
            var updatedAccount = await _employeeService.UpdateEmployeeAccountAsync(id, accountDto);
            if (updatedAccount is null)
                return NotFound("Account not found");
            return Ok(updatedAccount);
        }
    }
}
