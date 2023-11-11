using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Quizzes
{
    public class IndexModel : PageModel
    {
        [Authorize]
        public IActionResult OnGet()
        {
            return RedirectToPage("../Library/Index");
        }
    }
}
