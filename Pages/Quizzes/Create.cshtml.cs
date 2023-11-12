using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Acuedify.Pages.Quizzes
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

        public Quiz? quiz{ get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(Quiz quiz)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check
           
            this.quiz = quiz;
            this.quiz.UserId = userID;
            if (ModelState.IsValid)
            {
                _context.Add(this.quiz);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Library/Index");
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
