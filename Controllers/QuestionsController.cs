using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Acuedify.Controllers
{

    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly AppDBContext _context;
        String? userId;


        public QuestionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            if ((userId = getUserId()) == null) { return authErrorView(); }
            return _context.Question != null ?
                          View(await _context
                          .Question
                          .Where(q => q.UserId == userId)
                          .ToListAsync()) 
                          :Problem("Entity set 'AppDBContext.Question'  is null.");
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); }
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Where(q => q.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewBag.QuizIds = GetQuizIdsAsSelectListItems(); //what this do?
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] // what this do
        public async Task<IActionResult> Create([Bind("Id,Term,Definition,QuizId,UserId")] Question question)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); }

            if (ModelState.IsValid)
            {
                question.UserId = userId;
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); }

            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.QuizIds = GetQuizIdsAsSelectListItems(question.QuizId);

            return View(question);
        }
        
        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Term,Definition,QuizId,UserId")] Question question) //Hows this different from the previous Edit comment pls
        {
            if (id != question.Id) // maybe there is a better error page than 404
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    question.UserId = userId;
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id)) //maybe question you tried to edit doesnt exist?
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Quizzes", new { id = question.QuizId });
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); } //auth check

            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return View("ErrorView", "Question not foun");
            }

            // Ownership check
            if (question.UserId != userId){ return View("ErrorView", "You dont have access to this question"); } 

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {
            //no need for auth check since DeleteConfirmed accessible only from authchecked Delete action, no?
            if (_context.Question == null)
            {
                return Problem("Entity set 'AppDBContext.Question'  is null.");
            }
            var question = await _context.Question.FindAsync(id);
            if (question != null)
            {
                _context.Question.Remove(question);
            }

            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id) //idk if needs auth?
        {
          return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private List<SelectListItem> GetQuizIdsAsSelectListItems(int selectQuizId = -1) //idk if needs auth?
        {
            return ViewBag.QuizIds = _context.Quizzes
                .Select(q => q.Id)
                .Select(id => new SelectListItem(id.ToString(), id.ToString(), id == selectQuizId))
                .ToList();
        }

        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private ViewResult authErrorView()
        {
            return View("ErrorView", "You are not logged in (userId = null)");
        }
    }
}
