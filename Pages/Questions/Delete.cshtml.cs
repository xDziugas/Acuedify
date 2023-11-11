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
    public class DeleteModel : PageModel
    {
        private readonly AppDBContext _context;
        private string? userID;

        public DeleteModel(AppDBContext context)
        {
            _context = context;
        }
        public Question? Question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            if (id == null)
            {
                return errorPage("@Questions/Delete - not provided with Id");
            }
            if (_context.Question == null)
            {
                return errorPage("@Questions/Delete - Something wrong with the database of Questions.");
            }

            Question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Question == null)
            {
                return errorPage("@Questions/Delete - Could not fetch question for deletion" );
            }
            if (Question.UserId != userID) // Question access check
            {
                return errorPage("@Questions/Delete - You do not have access to this quiz.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            if (_context.Question == null)
            {
                return errorPage("@Questions/Delete - Something wrong with the database of Questions.");
            }

            if (question != null)
            {
                if (question.UserId != userID) // Question access check
                {
                    return errorPage("@Questions/Delete - You do not have access to this quiz.");
                }
                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
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
