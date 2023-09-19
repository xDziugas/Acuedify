using Acuedify.Data;
using Acuedify.Models;
using Microsoft.AspNetCore.Mvc;

namespace Acuedify.Controllers
{
	public class LibraryController : Controller
	{
		private readonly AppDBContext _db;
		private User? user;
		private int userId = 1;

        public LibraryController(AppDBContext db)
        {
            _db = db;
        }


		// GET: LibraryController
		[HttpGet]
        public ActionResult Index()
		{

			try
			{
				user = _db.User.Find(userId);
			}
			catch
			{
				return NotFound();
			}
			

			if(user == null)
			{
				return NotFound();
			}

			//???
			var userFolders = _db.User.Where(u => u.Id == userId).Select(u => u.Folders).FirstOrDefault();
			var userQuizzes = _db.User.Where(u => u.Id == userId).Select(u => u.Quizzes).FirstOrDefault();
			var favourites = new List<Quiz>();

			//todo: also check folders??
            if (userQuizzes != null) 
			{
				foreach (Quiz quiz in userQuizzes)
				{
					if (quiz.isFavorite)
					{
						favourites.Add(quiz);
					}
				}
			}

			LibraryDetails library = new LibraryDetails(folders: userFolders, quizzes: userQuizzes, favourites: favourites);

            return View(library);
		}

		//implement later
		public ActionResult CreateFolder()
		{ 
			return View();
		}

		// POST: edit folder/ later
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditFolder(int? id)
		{
			return View();
		}

		//GET, todo: make it better
		public IActionResult DeleteQuiz(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var quizToDelete = _db.Find<Quiz>(id);

			if (quizToDelete == null)
			{
				return NotFound();
			}

			return View(quizToDelete);
		}

		// Post
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteQuiz(Quiz? obj)
		{
			if (obj == null)
			{
				return NotFound();
			}

			try
			{
				_db.Remove<Quiz>(obj);
				_db.SaveChanges();
			}
			catch
			{
				return View(obj);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
