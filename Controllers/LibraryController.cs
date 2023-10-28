using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acuedify.Controllers
{
	[Route("Library")]
	public class LibraryController : Controller
	{
		private readonly ILibraryService _libraryService;

		public LibraryController(ILibraryService libraryService)
		{
			_libraryService = libraryService;
		}

		// GET:
		[HttpGet]
		public ActionResult Index()
		{

			var userFolders = _libraryService.GetUserFolders();
			var userQuizzes = _libraryService.GetUserQuizzes();
			var favourites = new List<Quiz>();

			if (userQuizzes == null)
			{
				return View("ErrorView", "Could not retrieve quizzes from database!");
			}


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
		public IActionResult GetQuiz(int id)
		{
			var quiz = _libraryService.GetUserQuiz(id);
			return View(quiz);
		}

		//POST
		public IActionResult CreateQuiz()
		{
			return View();
		}

		// PUT
		[HttpPut("{id}")]
		public IActionResult UpdateQuiz(Quiz quiz)
		{
			_libraryService.UpdateUserQuiz(quiz);
			return View();
		}

		//GET
		[HttpGet("{id}")]
		public IActionResult DeleteQuiz(int id)
		{
			var quizToDelete = _libraryService.GetUserQuiz(id);

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
			var result = _libraryService.DeleteUserQuiz(id);
			if (!result)
			{
				return View("ErrorView", "Failed to delete quiz from database!");
			}

			return RedirectToAction("Index");
		}

		[HttpGet("/fav")]
		public IActionResult ToggleFavorite(int id)
		{
			var quiz = _libraryService.GetUserQuiz(id);
			if (quiz != null)
			{
				quiz.isFavorite = !quiz.isFavorite;
				_libraryService.UpdateUserQuiz(quiz);
			}

			return RedirectToAction("Index");
		}
	}
}
