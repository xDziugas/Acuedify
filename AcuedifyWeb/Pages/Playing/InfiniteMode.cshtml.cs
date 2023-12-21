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
    public class InfiniteModeModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        private readonly IPlayingService _playingService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public InfiniteModeModel(ILibraryService libraryService, IPlayingService playingService,
            IAuthService authService, IErrorService errorService)
        {
            _libraryService = libraryService;
            _playingService = playingService;
            _authService = authService;
            _errorService = errorService;
        }

        public QuizStatistics? Statistics { get; set; }

        public IActionResult OnGet(int quizId)
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

            Statistics = _playingService.InitQuizStatistics(
                flashcards: flashcards,
                flashcardSet: flashcardSet
                );

            if (Statistics != null && Statistics.Quiz != null)
            {
                Statistics.Quiz.Questions = _playingService.ShuffleByDifficulty(Statistics.Quiz.Questions);
            }

            _playingService.SetToSession<QuizStatistics>(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session,
                details: Statistics
                );

            return Page();
        }

        //todo: delete questionId
        public IActionResult OnGetNextFlashCardPartial(int quizId, int questionId)
        {
            var statistics = _playingService.GetFromSession<QuizStatistics>(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session
            );

            statistics.TotalSolved = questionId;

            int lastQuestionId = statistics.CurrentIndex;
            int nextQuestionId = DetermineNextQuestionId(statistics, lastQuestionId);

            statistics.CurrentIndex = nextQuestionId;

            _playingService.SetToSession<QuizStatistics>(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session,
                details: statistics
            );

            if (!_playingService.isValid(statistics))
            {
                return Partial("ErrorView", "quiz is null");
            }

            return new PartialViewResult
            {
                ViewName = "_FlashcardContent",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Question>(ViewData, statistics?.Quiz?.Questions[statistics.CurrentIndex])
            };
        }

        //Updates statistics before getting next flashcard
        public JsonResult OnPostSubmitQuizResults([FromBody] int questionId, [FromBody] bool isCorrect)
        {
            var statistics = _playingService.GetFromSession<QuizStatistics>(
                SessionKey: Constants.PlayingSessionKey,
                session: HttpContext.Session
            );

            _playingService.UpdateWeight(statistics, questionId, isCorrect);
            return new JsonResult(new { success = true });
        }

        //todo: rename to index
        //gets next flashcard based on weight
        private int DetermineNextQuestionId(QuizStatistics statistics, int lastQuestionId)
        {
            Random random = new Random();
            double totalWeight = statistics.Weight.Sum();
            double choice = random.NextDouble() * totalWeight;

            double sum = 0;
            int idx = 0;
            for (int i = 0; i < statistics.Questions.Count; i++)
            {
                sum += statistics.Weight[i];
                if (sum >= choice)
                {
                    idx = i;
                    break;
                }
            }

            // If the chosen index is the same as the last one and there's more than one question,
            // adjust to the next question (or loop back to the first one)
            if (lastQuestionId == idx && statistics.Questions.Count > 1)
            {
                idx = (idx + 1) % statistics.Questions.Count;
            }

            return idx;
        }

    }
}
