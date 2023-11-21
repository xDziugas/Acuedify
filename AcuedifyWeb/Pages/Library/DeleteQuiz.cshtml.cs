using Acuedify.Models;
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
        private readonly UserManager<AcuedifyUser> _userManager;
        private string? userID;

        public DeleteQuizModel(ILibraryService libraryService, UserManager<AcuedifyUser> userManager)
		{
			this._libraryService = libraryService;
            this._userManager = userManager;
        }
		public Quiz Quiz { get; set; }

		public IActionResult OnGet(int id)
		{
            if ((userID = getUserId()) == null) { authErrorPage(); }

            Quiz = _libraryService.GetUserQuiz(id);
			if (Quiz.UserId != userID)
            {
                RedirectToPage("../Error", new { errormessage = "@Library/DeleteQuiz - You do not have access to this quiz." });
            }
			if (Quiz == null)
			{
                RedirectToPage("../Error", new { errormessage = "@Library/DeleteQuiz - Couldn't fetch quiz." });
            }

			return Page();
		}

		
		public IActionResult OnPostConfirm(int id)
		{
            //not sure if it is possible to delete other users quizzes
            //does [ValidateAntiForgeryToken] above the DeleteQuizModel protect from that?
            var result = _libraryService.DeleteUserQuiz(id);
			if (!result)
			{
                RedirectToPage("../Error", new { errormessage = "@Library/DeleteQuiz - Failed to delete quiz from database!" });
				return Page();
			}

			return RedirectToPage("Index");
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
    }
}
