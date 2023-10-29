using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Library
{
    public class IndexModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        public IndexModel(ILibraryService libraryService)
        {
            this._libraryService = libraryService;
        }


        
        public IEnumerable<Folder> Folders { get; set; }
        
        public IEnumerable<Quiz> Quizzes { get; set; }
        
		public IEnumerable<Quiz> Favourites { get; set; }


        public ActionResult OnGet()
        {

            this.Folders = _libraryService.GetUserFolders();
            this.Quizzes = _libraryService.GetUserQuizzes();
            var favourites = new List<Quiz>();
            if (Quizzes == null)
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve quizzes from database!");
                return Page();
            }


            foreach (Quiz quiz in Quizzes)
            {
                if (quiz.isFavorite)
                {
                    favourites.Add(quiz);
                }
            }
            this.Favourites = favourites;

            return Page();
        }
        public ActionResult OnGetToggleFavorite(int id)
		{
			var quiz = _libraryService.GetUserQuiz(id);
			if (quiz != null)
			{
				quiz.isFavorite = !quiz.isFavorite;
				_libraryService.UpdateUserQuiz(quiz);
			}

            return Page();
		}
	}
}
