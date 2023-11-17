using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
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
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public IndexModel(ILibraryService libraryService, IPlayingService playingService,
            IAuthService authService, IErrorService errorService)
		{
			_libraryService = libraryService;
			_playingService = playingService;
            _authService = authService;
            _errorService = errorService;
        }

		public PlayDetails? Details { get; set; }


		public IActionResult OnGet(int quizId, int questionId)
        {
			if (questionId == 0) // Initial loading of the screen with the first flashcard
			{
				var flashcardSet = _libraryService.GetUserQuiz(quizId);


                //flashcardSet authorization check
                if (!_authService.Authorized(flashcardSet)) { return Forbid(); }


                var flashcards = _libraryService.GetQuizQuestions(quizId);
                

                if (flashcards == null) 
				{
                    return _errorService.ErrorPage(this, "questions not found");
                }

				if (!flashcards.Any())
				{
					return RedirectToPage("/Library/Index");
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
                // Quiz access check
                if (Details == null)
                {
                    return _errorService.ErrorPage(this, "quiz not initialized");
                }

                //if quiz starts from 2 or further question 
                //authorization check
				if (!_authService.Authorized(Details.Quiz)) { return Forbid(); }


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
                    return _errorService.ErrorPage(this, "details invalid");
                }
				return Page();
			}

		}
    }
}
