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
    public class DetailsModel : PageModel
    {
        private readonly AppDBContext _context;
        private string? userID;

        public DetailsModel(AppDBContext context)
        {
            _context = context;
        }

        public Question? question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            if (id == null)
            {
                return errorPage("@Questions/Edit - not provided with Id");
            }

            if (_context.Question == null)
            {
                return errorPage("@Questions/Details - No questions found");
            }

            question = await _context.Question.FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return errorPage("@Questions/Details - Question " + id + " not found");
            }

            if (question.UserId != userID) // Question access check
            {
                return errorPage("@Questions/Details - You do not have access to this question.");
            }

            return Page();
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
