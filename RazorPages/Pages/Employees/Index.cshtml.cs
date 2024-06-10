using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Employees
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class IndexModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public IndexModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IList<EmployeeOnlyDto> Employees { get; set; }

        public async Task OnGetAsync()
        {
            Employees = (IList<EmployeeOnlyDto>)await _employeeService.GetAllEmployeesAsync();
        }
    }
}
