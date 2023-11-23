using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public CreateModel(AppDBContext context, IQuestionsService questionsService,
            IAuthService authService, IErrorService errorService)
        {
            _context = context;
            _questionsService = questionsService;
            _authService = authService;
            _errorService = errorService;
        }

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public IActionResult OnGet()
        {
            String? userId = _authService.GetUserId();

            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(-1, userId);
            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {

            if (question == null)
            {
                return _errorService.ErrorPage(this, "created question not found");
            }

            question.UserId = _authService.GetUserId();

            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
