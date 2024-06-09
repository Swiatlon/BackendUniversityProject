using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorPages.Pages.Employees
{
    [Authorize(Roles = "User")]
    public class IndexModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public IndexModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IList<EmployeeFullDto> Employees { get; set; }

        public async Task OnGetAsync()
        {
            Employees = (IList<EmployeeFullDto>)await _employeeService.GetAllEmployeesAsync();
        }
    }
}
