using Acuedify.Models;
using Acuedify.Services.Auth;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Library;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Library
{
    [ValidateAntiForgeryToken]
    [Authorize]
    public class DeleteQuizModel : PageModel
    {
		private readonly ILibraryService _libraryService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public DeleteQuizModel(ILibraryService libraryService,
            IAuthService authService, IErrorService errorService)
		{
			_libraryService = libraryService;
            _authService = authService;
            _errorService = errorService;
        }
		public Quiz? Quiz { get; set; }

		public IActionResult OnGet(int id)
		{
            Quiz = _libraryService.GetUserQuiz(id);
            
            //authorization check
            if (!_authService.Authorized(Quiz)) { return Forbid(); }

            if (Quiz == null)
			{
                _errorService.ErrorPage(this, "quiz not found");
            }

			return Page();
		}

		
		public IActionResult OnPostConfirm(int id)
		{
            Quiz quiz = _libraryService.GetUserQuiz(id);
            //authorization check
            if (!_authService.Authorized(quiz)) { return Forbid(); }

            var result = _libraryService.DeleteUserQuiz(id);
			if (!result)
			{
                _errorService.ErrorPage(this, "couldn't delete quiz");
                return Page();
			}

			return RedirectToPage("Index");
		}
    }
}
