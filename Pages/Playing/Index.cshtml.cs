using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Acuedify.Services.Playing;
using Acuedify.Services.Playing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Playing
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly ILibraryService _libraryService;
		private readonly IPlayingService _playingService;

		public IndexModel(ILibraryService libraryService, IPlayingService playingService)
		{
			_libraryService = libraryService;
			_playingService = playingService;
		}

		public PlayDetails? Details { get; set; }


		public IActionResult OnGet(int quizId, int questionId)
        {
			if (questionId == 0) // Initial loading of the screen with the first flashcard
			{
				var flashcardSet = _libraryService.GetUserQuiz(quizId);
				var flashcards = _libraryService.GetQuizQuestions(quizId);

				if (flashcards == null) // <-- always false dk why
				{
					return RedirectToPage("../Error", "no quiz found by id");
				}

				if (!flashcards.Any())
				{
					return RedirectToPage("../Library/Index");
				}

				//details unused by view pls remove
				Details = _playingService.InitPlayDetails(
					flashcards: flashcards,
					flashcardSet: flashcardSet
					);

				_playingService.SetToSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session,
					details: Details
					);
				return Page();
			}
			else // Loading of any other flashcard
			{
				//add quiz uninitialized error ( for vlad by vlad ) 
				Details = _playingService.GetFromSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session
				);

				Details.CurrentIndex = questionId;

				_playingService.SetToSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session,
					details: Details
					);

				if (!_playingService.isValid(Details))
				{
					return RedirectToPage("../Error", "quiz is null");
				}
				return Page();
			}

		}
	}
}
