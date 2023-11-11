using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Pages.Questions
{
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;

        public EditModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Id is null" });
            }
            if (_context.Question == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Question db context is empty" });
            }

            question = await _context.Question.FindAsync(id);
            
            if (question == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Question is null" });
            }

            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(question.QuizId);

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question) {

            this.question = question;
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
                        return RedirectToPage("../Error", new { errormessage = "Question Id: " + question.Id + " doesnt exist."});
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("Edit", new { id = question.Id });
            }
            return Page();
        }
    }
}
