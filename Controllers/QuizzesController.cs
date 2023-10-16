using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Acuedify.Controllers
{

    [Authorize]
    public class QuizzesController : Controller
    {
        private readonly AppDBContext _context;
        String? userId;
        public QuizzesController(AppDBContext context)
        {
            _context = context;
        }


        // GET: Quizzes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,isFavorite, UserId")] Quiz quiz)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); } //auth check

            if (ModelState.IsValid)
            {
                quiz.UserId = userId;
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Library");
            }

            return View(quiz);
        }


        // GET: Quizzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); } //auth check

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

            // Ownership check
            if (quiz.UserId != userId) { return View("ErrorView", "You do not have acces to this Quiz"); }

            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UserId")] Quiz quiz)
        {
            if ((userId = getUserId()) == null) { return authErrorView(); } //auth check

            if (id != quiz.Id)
            {
                return NotFound();
            }
            // Ownership check
            if (quiz.UserId != userId) { return View("ErrorView", "You do not have access to this quiz"); }

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
