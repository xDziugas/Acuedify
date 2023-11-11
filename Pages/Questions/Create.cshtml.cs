using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acuedify.Pages.Questions
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

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public void OnGet()
        {
            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(-1);
            Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
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
