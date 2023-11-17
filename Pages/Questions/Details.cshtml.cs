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
    public class DetailsModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public DetailsModel(AppDBContext context, IAuthService authService,
            IErrorService errorService)
        {
            _context = context;
            _authService = authService;
            _errorService = errorService;
        }

        public Question? question { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {            
            if (id == null)
            {
                return _errorService.ErrorPage(this, "no id provided");
            }

            if (_context.Question == null)
            {
                return _errorService.ErrorPage(this, "questions not found");
            }

            question = await _context.Question.FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return _errorService.ErrorPage(this, "question not found");
            }

            //quiz authorization check
            if (!_authService.Authorized(question)) { return Forbid(); }

            return Page();
        }
    }
}
