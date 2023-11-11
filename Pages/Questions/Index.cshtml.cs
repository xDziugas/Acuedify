using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly AppDBContext _context;
        private string? userID;

        public IndexModel(AppDBContext context)
		{
			_context = context;
		}

        public IEnumerable<Question> Questions { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            // Logged in check
            if ((userID = getUserId()) == null) { return authErrorPage(); }

            if (_context.Question != null)
            {
                Questions = await _context.Question
                    .Where(question => question.UserId == userID) // Question access filter
                    .ToListAsync();
                return Page();
            }
            else
            {
                return errorPage("@Questions/Index - Something wrong with the database of Questions.");
            }

        }





        //auth helper functions
        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private RedirectToPageResult authErrorPage()
        {
            return RedirectToPage("../Error", new { errormessage = "You are not logged in (userId = null)" });
        }
        private RedirectToPageResult errorPage(String errorMessage)
        {
            return RedirectToPage("../Error", new { errormessage = errorMessage });
        }
    }
}
