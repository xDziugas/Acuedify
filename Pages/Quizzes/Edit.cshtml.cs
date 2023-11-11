using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Pages.Quizzes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;

        public EditModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

        public Quiz? quiz { get; set; }

        public async Task<IActionResult> OnGet(int? quizId)
        {
            if (quizId == null || _context.Quizzes == null)
            {
                return RedirectToPage("../Error", new { errormessage = "@Pages/Edit - Database is empty or didnt provide quizid" });
            }

            quiz = await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return RedirectToPage("../Error", "Quiz not found."); ;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, Quiz quiz)
        {
            this.quiz = quiz;
            if (id != quiz.Id)
            {
                return RedirectToPage("../Error", "Editted quiz not found.");
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
                        return RedirectToPage("../Error", "Quiz doesn't exist.");
                    }
                    else
                    {
                        return RedirectToPage("../Error", "Unknown error");
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
    }
}
