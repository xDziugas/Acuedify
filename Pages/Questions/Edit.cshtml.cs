using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acuedify.Services.Auth.Interfaces;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public EditModel(AppDBContext context, IQuestionsService questionsService,
            IAuthService authService, IErrorService errorService)
        {
            _context = context;
            _questionsService = questionsService;
            _authService = authService;
            _errorService = errorService;
        }

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null) 
            {
                return _errorService.ErrorPage(this, "quizzes not found");
            }
            // is this required below
            if (_context.Question == null) 
            {
                return _errorService.ErrorPage(this, "questions not found");
            }
        
            question = await _context.Question.FindAsync(id);

            if (question == null)
            {
                return _errorService.ErrorPage(this, "question not found");
            }

            //authorization check
            if (!_authService.Authorized(question)) { return Forbid(); }


            // fix with quiz name 
            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(question.QuizId, _authService.GetUserId());

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            this.question = question;

            //authorization check
            if (!_authService.Authorized(question)) { return Forbid(); }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_questionsService.QuestionExists(question.Id))
                    {
                        return _errorService.ErrorPage(this, "question " + question.Id + " doesnt exist.");
                    }
                    else
                    {
                        return _errorService.ErrorPage(this, "failed to save");
                    }
                }
                return RedirectToPage("Edit", new { id = question.Id });
            }
            return Page();
        }
    }
}
