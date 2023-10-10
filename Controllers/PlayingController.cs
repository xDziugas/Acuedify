﻿using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Acuedify.Services.Playing;
using Acuedify.Services.Playing.Interfaces;
using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;

namespace Acuedify.Controllers
{
    [Route("Play")]
    public class PlayingController : Controller
	{
        private readonly ILibraryService _libraryService;
		private readonly IPlayingService _playingService;

		public PlayingController(ILibraryService libraryService, IPlayingService playingService)
        {
            _libraryService = libraryService;
            _playingService = playingService;
        }

        // GET:
        [HttpGet]
        public IActionResult Index(int quizId, int questionId)
        {
		    PlayDetails details;

            if (questionId == 0) // Initial loading of the screen with the first flashcard
            {
                var flashcardSet = _libraryService.GetUserQuiz(quizId);
				var flashcards = _libraryService.GetQuizQuestions(quizId);

				if (flashcards == null)
				{
					return View("ErrorView", "no quiz found by id");
				}

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

                if (_playingService.isValid(details))
                {
					return View("ErrorView", "quiz is null or quiz is finished");
				}
            }

            return View(details);
        }
    }
}
