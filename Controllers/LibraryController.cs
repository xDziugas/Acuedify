using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public LibraryController(ILibraryService libraryService, 
			UserManager<AcuedifyUser> userManager,
            SignInManager<AcuedifyUser> signInManager)
		{
			_libraryService = libraryService;
            _userManager = userManager;
            _signInManager = signInManager;
		}

		// GET:
		[HttpGet]
		public ActionResult Index()
		{
			String userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
;
            var userFolders = _libraryService.GetUserFolders();
			var userQuizzes = _libraryService.GetUserQuizzes(userID);
			var favourites = new List<Quiz>();

			if (userQuizzes == null)
			{
				return View("ErrorView", "User has no quizzes");
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
		public async Task<IActionResult> GetQuiz(int id)
		{
			var user = await _userManager.GetUserAsync(User);

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
	}
}
