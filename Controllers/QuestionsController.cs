using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acuedify.Data;
using Acuedify.Models;
using System.Dynamic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Acuedify.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly AppDBContext _context;

        public QuestionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            
            var questionList = _context.Question != null ? 
                await _context.Question.ToListAsync() : null;
            if ( questionList == null ) { return View("ErrorView", "Couldn't find any questions."); }
            var quizList = _context.Quizzes != null ?
                await _context.Quizzes.ToListAsync() : null;
            if (quizList == null) { return View("ErrorView", "Couldn't find any quizzes."); }

            List<QQViewModel> qqmodels = new List<QQViewModel>(); ;
            foreach (Question question in questionList)
            {
                Quiz? quiz = quizList.FirstOrDefault(quiz => quiz.Id == question.QuizId);
                if(quiz == null) { return View("ErrorView", "Couldnt find a quiz for each question."); }
                qqmodels.Add(new QQViewModel
                {
                    Question = question,
                    Quiz = quiz
                });
            }
            return View(qqmodels);
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null) { return NotFound(); }

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == question.QuizId);
            if (quiz == null) { return NotFound(); }


            var qqmodel = new QQViewModel();
            qqmodel.Question = question;
            qqmodel.Quiz = quiz;

            return View(qqmodel);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewBag.QuizIds = GetQuizIdsAsSelectListItems();
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Term,Definition,QuizId")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == question.QuizId);
            if (quiz == null) { return NotFound(); }


            var qqmodel = new QQViewModel();
            qqmodel.Question = question;
            qqmodel.Quiz = quiz;

            var one = _context.Quizzes
                .ToList()
                .Select(quiz => quiz.Title)
                .ToList();
            ViewBag.QuizNames = _context.Quizzes
                .ToList()
                .Select(quiz => quiz.Title)
                .Select(title => new SelectListItem(title, title))
                .ToList();
            var two = GetQuizIdsAsSelectListItems(id);

            return View(qqmodel);
        }
        
        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QQViewModel qqmodel)
        {
            //find the referenced quiz
            Quiz? newRefQuiz = (Quiz?)_context.Quizzes
                .Where(quiz => quiz.Title.Equals(qqmodel.Quiz.Title))
                .ToList()
                .First();

            qqmodel.Quiz = newRefQuiz;
            qqmodel.Question.QuizId = newRefQuiz.Id;

            if (!ModelState.IsValid) { return View("ErrorView", qqmodel.ToString()); }
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(qqmodel.Question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(qqmodel.Question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (qqmodel.Quiz == null) { return View("ErrorView", "sumtingwong"); }
                return RedirectToAction("Edit", "Quizzes", new { id = qqmodel.Question.QuizId });
            }
            return View(qqmodel);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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

        // GET: Questions/CreateForQuiz/3
        public IActionResult CreateForQuiz(int id)
        {
            ViewBag.QuizIds = GetQuizIdsAsSelectListItems(id);
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForQuiz([Bind("Term,Definition,QuizId")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Quizzes", new { id = question.QuizId });
            }
            return View(question);
        }

        private bool QuestionExists(int id)
        {
          return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private List<SelectListItem> GetQuizIdsAsSelectListItems(int? selectQuizId = -1)
        {
            return ViewBag.QuizIds = _context.Quizzes
                .Select(q => q.Id)
                .Select(id => new SelectListItem(id.ToString(), id.ToString(), id == selectQuizId))
                .ToList();
        }

    }
}
