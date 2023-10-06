using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acuedify.Data;
using Acuedify.Models;

namespace Acuedify.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly AppDBContext _context;

        public QuizzesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Quizzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quizzes == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .Where(q => q.Id == id)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", new { id = quiz.Id });
            }
            return View(quiz);
        }

        private bool QuizExists(int id)
        {
          return (_context.Quizzes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
