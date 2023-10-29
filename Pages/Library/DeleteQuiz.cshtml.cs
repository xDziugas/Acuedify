using Acuedify.Models;
using Acuedify.Services.Library;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Library
{
    public class DeleteQuizModel : PageModel
    {
		private readonly ILibraryService _libraryService;
		public DeleteQuizModel(ILibraryService libraryService)
		{
			this._libraryService = libraryService;
		}
		public Quiz Quiz { get; set; }
		public IActionResult OnGet(int id)
		{
			Quiz = _libraryService.GetUserQuiz(id);

			if (Quiz == null)
			{
				ModelState.AddModelError(string.Empty, "Could not retrieve quizzes from database!");
				return Page();
			}

			return Page();
		}

		[ValidateAntiForgeryToken]
		public IActionResult OnPostConfirm(int id)
		{
			var result = _libraryService.DeleteUserQuiz(id);
			if (!result)
			{
				ModelState.AddModelError(string.Empty, "Failed to delete quiz from database!");
				return Page();
			}

			return RedirectToPage("Index");
		}
	}
}
