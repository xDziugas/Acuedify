using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Pages.Questions
{
    public class DetailsModel : PageModel
    {
        private readonly AppDBContext _context;

        public DetailsModel(AppDBContext context)
        {
            _context = context;
        }

        public Question? question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            System.Diagnostics.Debug.WriteLine("1");
            if (id == null || _context.Question == null)
            {
                return RedirectToPage("../Error", "No questions found");
            }

            question = await _context.Question.FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return RedirectToPage("../Error", "Question not found");
            }

            return Page();
        }
    }
}
