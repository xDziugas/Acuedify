using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IQuestionsService _questionsService;
        private string? userID;

        public EditModel(AppDBContext context, IQuestionsService questionsService)
        {
            _context = context;
            _questionsService = questionsService;
        }

        public Question? question { get; set; }
        public List<SelectListItem> QuizIds { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check
            if (id == null) 
            {
                return errorPage("@Questions/Edit - not provided with Id");
            }

            if (_context.Question == null) 
            {
                return errorPage("@Questions/Index - Something wrong with the database of Questions.");
            }
        
            question = await _context.Question.FindAsync(id);

            if (question == null)
            {
                return errorPage("@Questions/Index - Question not found.");
            }

            if (question.UserId != userID) // Question access check
            {
                return errorPage("@Questions/Index - You do not have access to this question.");
            }


            // fix with quiz name 
            QuizIds = _questionsService.GetQuizIdsAsSelectListItems(question.QuizId, userID);

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            if ((userID = getUserId()) == null) { return authErrorPage(); } // Logged in check

            this.question = question;

            if (question.UserId != userID) // Question access check
            {
                return errorPage("@Questions/Index - You do not have access to this question.");
            }

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
                        return errorPage("@Questions / Index - Question " + question.Id + " doesnt exist.");
                    }
                    else
                    {
                        return errorPage("@Questions / Index - Failed to update the question database.");
                    }
                }
                return RedirectToPage("Edit", new { id = question.Id });
            }
            return Page();
        }





        //auth helper functions
        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private RedirectToPageResult authErrorPage()
        {
            return RedirectToPage("../Error", new { errormessage = "You are not logged in (userId = null)" });
        }
        private RedirectToPageResult errorPage(String errorMessage)
        {
            return RedirectToPage("../Error", new { errormessage = errorMessage });
        }
    }
}
