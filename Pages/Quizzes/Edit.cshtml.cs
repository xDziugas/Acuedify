using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Acuedify.Pages.Quizzes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;
        private string? userID;

        public EditModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
        }

        public Quiz? quiz { get; set; }

        public async Task<IActionResult> OnGet(int? quizId)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            if (quizId == null)
            {
                return errorPage("@Quizzes/Edit - not provided with Id.");
            }

            if (_context.Quizzes == null)
            {
                return errorPage("@Quizzes/Edit - Something went wrong with Quizzes database." );
            }

            quiz = await _context.Quizzes
                .Where(quiz => quiz.UserId == userID) // quiz access check
                .Where(q => q.Id == quizId)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return errorPage("@Quizzes/Edit - Quiz not found or you do not have access");
            }

            return Page();
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //FIXXXXX
        public async Task<IActionResult> OnPost(int id, Quiz quiz)
        {   
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check
            System.Diagnostics.Debug.WriteLine(quiz.Id);
            System.Diagnostics.Debug.WriteLine(id);
            //fix this
            this.quiz = quiz;
            if (id != quiz.Id)
            {
                return errorPage("@Quizzes/Edit - Editted quiz not found."); // when does that happen? (for vlad by vlad)
            }

            if(quiz.UserId != userID) 
            {
                return errorPage("@Quizzes/Edit - You do not have access to this quiz");
                //belenkai gera daina per spotifaju pasileidau katik
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.Id))
                    {
                        return errorPage("@Quizzes/Edit - Quiz doesnt exist");
                    }
                    else
                    {
                        return errorPage("@Quizzes/Edit - Failed to update database");
                    }
                } 
                return RedirectToPage("Edit", new { id = quiz.Id });
            }
            return Page();
        }

        private bool QuizExists(int id)
        {
            return (_context.Quizzes?.Any(e => e.Id == id)).GetValueOrDefault();
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
