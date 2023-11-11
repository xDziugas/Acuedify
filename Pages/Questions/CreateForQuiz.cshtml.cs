using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class CreateForQuizModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;

        public CreateForQuizModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

        public Question? question { get; set; }


        public List<SelectListItem> QuizIds { get; set; }


        public IActionResult OnGet(int id)
        {
            System.Diagnostics.Debug.WriteLine("1");
            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(id);
            System.Diagnostics.Debug.WriteLine("2");
            return Page();
            System.Diagnostics.Debug.WriteLine("3");
        }


        public async Task<IActionResult> OnPost(Question question)
        {

            System.Diagnostics.Debug.WriteLine("4");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("5");
                _context.Add(question);
                System.Diagnostics.Debug.WriteLine("6");
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("7");
                return RedirectToPage("../Quizzes/Edit", new { quizId = question.QuizId });
                System.Diagnostics.Debug.WriteLine("8");
            }
            System.Diagnostics.Debug.WriteLine("9" );
            return Page();
        }

    }
}
