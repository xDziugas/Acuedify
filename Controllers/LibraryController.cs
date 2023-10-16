using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace Acuedify.Controllers
{
	[Route("Library")]
	[Authorize]
	public class LibraryController : Controller
	{
		private readonly ILibraryService _libraryService;
        private readonly UserManager<AcuedifyUser> _userManager;
        private readonly SignInManager<AcuedifyUser> _signInManager;
		private String? userId;

        public LibraryController(ILibraryService libraryService)
		{
			_libraryService = libraryService;
		}

		// GET:
		[HttpGet]
		public ActionResult Index()
		{
            if ((userId = getUserId()) == null) { return errorView(); }

            var userFolders = _libraryService.GetUserFolders(userId);

			var userQuizzes = _libraryService.GetUserQuizzes(userId);
            if (userQuizzes == null) { return View("ErrorView", "User has no quizzes"); }

            var favourites = new List<Quiz>();



			foreach (Quiz quiz in userQuizzes)
			{
				if (quiz.isFavorite)
				{
					favourites.Add(quiz);
				}
			}

			LibraryDetails library = new LibraryDetails(folders: userFolders, quizzes: userQuizzes, favourites: favourites);

			return View(library);
		}

		//GET
		public async Task<IActionResult> GetQuiz(int id)
		{
            if ((userId = getUserId()) == null) { return errorView(); }


            var quiz = _libraryService.GetUserQuiz(id, userId);
			if (quiz == null) { return View("ErrorView", "Quiz not found"); }

			return View(quiz);
		}

		//POST
		public IActionResult CreateQuiz()
		{
			return View();
		}

		// PUT
		[HttpPut("{id}")] // needed??
		public IActionResult UpdateQuiz(Quiz quiz)
		{
            if ((userId = getUserId()) == null) { return errorView(); }

            _libraryService.UpdateUserQuiz(quiz, userId);
			return View();
		}

		//GET
		[HttpGet("{id}")]
		public IActionResult DeleteQuiz(int id)
		{
            if ((userId = getUserId()) == null) { return errorView(); }

            var quizToDelete = _libraryService.GetUserQuiz(id, userId);

			if (quizToDelete == null)
			{
				return View("ErrorView", "Could not find the quiz!");
			}

			return View(quizToDelete);
		}

		// POST:
		[HttpPost("{id}")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirm(int id)
		{
            if ((userId = getUserId()) == null) { return errorView(); }

            var result = _libraryService.DeleteUserQuiz(id, userId);
			if (!result)
			{
				return View("ErrorView", "Failed to delete quiz from database!");
			}

			return RedirectToAction("Index");
		}

		private String? getUserId()
		{
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

		private ViewResult errorView()
		{
            return View("ErrorView", "You are not logged in (userId = null)");
        }
	}
}
