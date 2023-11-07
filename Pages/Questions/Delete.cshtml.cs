using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Pages.Questions
{
    public class DeleteModel : PageModel
    {
        private readonly AppDBContext _context;

        public DeleteModel(AppDBContext context)
        {
            _context = context;
        }
        public Question? question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Could not fetch question for deletion" });
            }

            question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Could not fetch question for deletion" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            if (_context.Question == null)
            {
                return RedirectToPage("../Error", new { errormessage = "Entity set 'AppDBContext.Question'  is null." });
            }

            if (question != null)
            {
                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
