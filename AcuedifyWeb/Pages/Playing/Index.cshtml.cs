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
					return RedirectToPage("../Library/Index");
				}

            //details unused by view pls remove
            Details = _playingService.InitPlayDetails(
                flashcards: flashcards,
                flashcardSet: flashcardSet
                );

            if (Details != null && Details.Quiz != null)
            {
                Details.Quiz.Questions = _playingService.ShuffleByDifficulty(Details.Quiz.Questions);
            }

            _playingService.SetToSession(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session,
                details: Details
                );

            return Page();
        }

        public IActionResult OnGetNextFlashCardPartial(int quizId, int questionId)
        {
            var details = _playingService.GetFromSession(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session
            );

            details.CurrentIndex = questionId;

            _playingService.SetToSession(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session,
                details: details
            );

				if (!_playingService.isValid(details))
				{
                   return Partial("ErrorView", "quiz is null");
                }

            return new PartialViewResult
            {
                ViewName = "_FlashcardContent",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Question>(ViewData, details?.Quiz?.Questions[details.CurrentIndex])
            };
        }

        public JsonResult OnPostSubmitQuizResults([FromBody] QuizResultsModel results)
        {
            _libraryService.UpdateQuizResult(results);

            //updates lastPlayed property
            _libraryService.UpdateProperties(results.quizId);

            return new JsonResult(new { success = true });
        }
    }
}
