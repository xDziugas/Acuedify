using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Acuedify.Services.Playing;
using Acuedify.Services.Playing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Playing
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly ILibraryService _libraryService;
		private readonly IPlayingService _playingService;
        private string? userID;

        public IndexModel(ILibraryService libraryService, IPlayingService playingService)
		{
			_libraryService = libraryService;
			_playingService = playingService;
		}

		public PlayDetails? Details { get; set; }


		public IActionResult OnGet(int quizId, int questionId)
        {
			// Logged in check
            if ((userID = getUserId()) == null) { authErrorPage();}

            if (questionId == 0) // Initial loading of the screen with the first flashcard
			{
				var flashcardSet = _libraryService.GetUserQuiz(quizId);

				// Quiz access check
                if (flashcardSet.UserId != userID) 
				{ 
					return errorPage("@Playing - You do not have access to this quiz."); 
				} 

                var flashcards = _libraryService.GetQuizQuestions(quizId);
                

                if (flashcards == null) 
				{
                    return errorPage("@Playing - Unable to fetch quiz questions.");
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

                Details = _playingService.GetFromSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session
				);

                // Quiz access check
                if (Details == null)
                {
                    return errorPage("@Playing - Details not initialized.");
                }
                //if quiz starts from 2 or further question 
                if (Details.Quiz.UserId != userID)
                {
                    return errorPage("@Playing - You do not have access to this quiz.");
                }

                Details.CurrentIndex = questionId;

				_playingService.SetToSession(
					SessionKey: Constants.PlayingSessionKey,
					session: HttpContext.Session,
					details: Details
					);

				if (!_playingService.isValid(Details))
				{
					return errorPage("quiz is null");
				}
				return Page();
			}

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
            return new JsonResult(new { success = true });
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
        private RedirectToPageResult errorPage(String errorMessage)
        {
            return RedirectToPage("../Error", new { errormessage = errorMessage });
        }
    }
}
