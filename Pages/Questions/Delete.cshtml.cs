using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Acuedify.Pages.Questions
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public DeleteModel(AppDBContext context,
            IAuthService authService, IErrorService errorService)
        {
            _context = context;
            _authService = authService;
            _errorService = errorService;
        }
        public Question? Question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return _errorService.ErrorPage(this, "id not provided");
            }
            if (_context.Question == null)
            {
                return _errorService.ErrorPage(this, "questions not found");
            }

            Question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Question == null)
            {
                return _errorService.ErrorPage(this, "question not found");
            }

            //authorization check
            if (!_authService.Authorized(Question)) { return Forbid(); }

            return Page();
        }

        public async Task<IActionResult> OnPost(Question question)
        {
            
            if (_context.Question == null)
            {
                return _errorService.ErrorPage(this, "Questions not found");
            }

            if (question != null)
            {
                //authorization check
                if (!_authService.Authorized(question)) { return Forbid(); }

                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
