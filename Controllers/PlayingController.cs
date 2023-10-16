using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Acuedify.Services.Playing;
using Acuedify.Services.Playing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Acuedify.Controllers
{
	[Route("Play")]

    [Authorize]
    public class PlayingController : Controller
	{
		private readonly ILibraryService _libraryService;
		private readonly IPlayingService _playingService;
		String? userId;
		String? validCheckResult;

        public PlayingController(ILibraryService libraryService, IPlayingService playingService)
		{
			_libraryService = libraryService;
			_playingService = playingService;
		}

        // GET:
        [HttpGet]
        public IActionResult Index(int quizId, int questionId)
		{
			userId = getUserId();
			if (userId == null) { return authErrorView(); }
			if (_libraryService.GetUserQuiz(quizId, userId) == null) { return View("ErrorView", "You do not have access to this quiz"); }

            PlayDetails details;

            if (questionId == 0) // Initial loading of the screen with the first flashcard
			{

                var flashcardSet = _libraryService.GetUserQuiz(quizId, userId);
				var flashcards = _libraryService.GetQuizQuestions(quizId, userId);

				if (flashcards == null)
				{
					return View("ErrorView", "no quiz found by id");
				}

				flashcards.Shuffle();

				details = _playingService.InitPlayDetails(
					flashcards: flashcards,
					flashcardSet: flashcardSet
					);

				_playingService.SetToSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session,
					details: details

					);
			}
			else // Loading of any other flashcard
			{
				details = _playingService.GetFromSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session
					);

				details.CurrentIndex = questionId;

				_playingService.SetToSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session,
					details: details
					);

				if ((validCheckResult = _playingService.isValid(details)) != null)
				{
					return View("ErrorView", validCheckResult);
				}
			}

			return View(details);
		}
        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private ViewResult authErrorView()
        {
            return View("ErrorView", "You are not logged in (userId = null)");
        }
    }
}
