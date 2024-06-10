using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Employees
{
    [Authorize(Policy = "AdminOrUserPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public DetailsModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public EmployeeFullDto Employee { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(id);
        }
    }
}
