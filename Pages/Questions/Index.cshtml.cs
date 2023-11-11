using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly AppDBContext _context;

		public IndexModel(AppDBContext context)
		{
			_context = context;
		}

        public IEnumerable<Question> Questions { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Question != null)
            {
                Questions = await _context.Question.ToListAsync();
                return Page();
            }
            else
            {
                return RedirectToPage("../Error", "Entity set 'AppDBContext.Question'  is null.");
            }

        }
    }
}
