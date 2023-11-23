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
    public class IndexModel : PageModel
    {
		private readonly AppDBContext _context;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public IndexModel(AppDBContext context,
            IAuthService authService, IErrorService errorService)
		{
			_context = context;
            _authService = authService;
            _errorService = errorService;
        }

        public IEnumerable<Question> Questions { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            String? userId = _authService.GetUserId();

            if (_context.Question != null)
            {
                Questions = await _context.Question
                    .Where(question => question.UserId == userId)
                    //or
                    //.Where(question => _authService.Authorized(question))
                    .ToListAsync();
                return Page();
            }
            else
            {
                return _errorService.ErrorPage(this, "questions not found");
            }

        }
    }
}
