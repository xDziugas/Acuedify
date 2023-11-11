using Acuedify.Data;
using Acuedify.Models;
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
        private string? userID;

        public CreateModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public IActionResult OnGet()
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(-1, userID);
            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            if (question == null)
            {
                return errorPage("@Questions/Create - Could not fetch question from POST");
            }

            question.UserId = userID;

            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
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
