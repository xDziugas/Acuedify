using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Quizzes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IAuthService _authService;

        public CreateModel(AppDBContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public Quiz? quiz{ get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(Quiz quiz)
        {
            String? userId = _authService.GetUserId();
           
            this.quiz = quiz;
            this.quiz.UserId = userId;
            if (ModelState.IsValid)
            {
                _context.Add(this.quiz);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Library/Index");
            }

            return Page();
        }
    }
}
