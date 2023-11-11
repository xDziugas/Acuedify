using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acuedify.Pages.Quizzes
{
    public class CreateModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;

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
            this.quiz = quiz;
            if (ModelState.IsValid)
            {
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Library/Index");
            }

            return Page();
        }
    }
}
