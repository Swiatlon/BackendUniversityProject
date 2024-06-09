using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace RazorPages.Pages.Employees
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public CreateModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public EmployeeFullDto Employee { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _employeeService.AddEmployeeAsync(Employee);

            return RedirectToPage("./Index");
        }
    }
}
