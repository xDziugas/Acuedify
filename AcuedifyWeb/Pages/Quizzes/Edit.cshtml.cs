using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
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
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public EditModel(AppDBContext context, 
            IAuthService authService, IErrorService errorService)
        {
            _context = context;
            _authService = authService;
            _errorService = errorService;
        }

        public Quiz? quiz { get; set; }

        public async Task<IActionResult> OnGet(int? quizId)
        {
            String? userId = _authService.GetUserId(); 

            if (quizId == null)
            {
                return _errorService.ErrorPage(this, "not provided by id");
            }

            if (_context.Quizzes == null)
            {
                return _errorService.ErrorPage(this, "quizzes not found");
            }

            quiz = await _context.Quizzes
                .Where(quiz => quiz.UserId == userId) // quiz access check
                .Where(q => q.Id == quizId)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return _errorService.ErrorPage(this, "quiz not found");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int id, Quiz quiz)
        {   
            String? userId = _authService.GetUserId();

            //authorization check
            if (!_authService.Authorized(quiz)) { return Forbid(); }


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
                        return _errorService.ErrorPage(this, "quiz not found");
                    }
                    else
                    {
                        return _errorService.ErrorPage(this, "failed to edit quiz");
                    }
                }
                return RedirectToPage("Edit", new { quizId = quiz.Id });
            }
            return Page();
        }

        private bool QuizExists(int id)
        {
            return (_context.Quizzes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
