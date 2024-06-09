using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeOnlyDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeOnlyDto>>(employees);
        }

        public async Task<EmployeeFullDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");

            return _mapper.Map<EmployeeFullDto>(employee);
        }

        public async Task<EmployeeFullDto> AddEmployeeAsync(EmployeeFullDto employeeDto)
        {
            var employeeEntity = _mapper.Map<Employee>(employeeDto);
            var employee = await _employeeRepository.CreateAsync(employeeEntity);
            return _mapper.Map<EmployeeFullDto>(employee);
        }

        public async Task<EmployeeFullDto> UpdateEmployeeAsync(EmployeeFullDto employeeDto)
        {
            var employeeEntity = await _employeeRepository.GetByIdAsync(employeeDto.Id);
            if (employeeEntity is null) throw new KeyNotFoundException("Employee not found");

            _mapper.Map(employeeDto, employeeEntity);

            var updatedEmployee = await _employeeRepository.UpdateAsync(employeeEntity);
            return _mapper.Map<EmployeeFullDto>(updatedEmployee);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");
            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<AddressOnlyDto> GetEmployeeAddressAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");

            return _mapper.Map<AddressOnlyDto>(employee.Address);
        }

        public async Task<AddressOnlyDto> UpdateEmployeeAddressAsync(Guid id, AddressOnlyDto addressDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");

            var addressEntity = _mapper.Map<Address>(addressDto);
            employee.Address = addressEntity;

            await _employeeRepository.UpdateAsync(employee);
            return _mapper.Map<AddressOnlyDto>(addressEntity);
        }

        public async Task<AccountOnlyDto> GetEmployeeAccountAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");

            return _mapper.Map<AccountOnlyDto>(employee.Account);
        }

        public async Task<AccountOnlyDto> UpdateEmployeeAccountAsync(Guid id, AccountOnlyDto accountDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null) throw new KeyNotFoundException("Employee not found");

            var accountEntity = _mapper.Map<Account>(accountDto);
            employee.Account = accountEntity;

            await _employeeRepository.UpdateAsync(employee);
            return _mapper.Map<AccountOnlyDto>(accountEntity);
        }
    }
}
