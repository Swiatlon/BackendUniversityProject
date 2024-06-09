using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public EditModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public EmployeeFullDto Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _employeeService.UpdateEmployeeAsync(Employee);

            return RedirectToPage("./Index");
        }
    }
}
