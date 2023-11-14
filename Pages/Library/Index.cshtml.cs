using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Library
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        private readonly UserManager<AcuedifyUser> _userManager;
        private string? userID;

        public IndexModel(ILibraryService libraryService, UserManager<AcuedifyUser> userManager)
        {
            this._libraryService = libraryService;
            this._userManager = userManager;
        }

        public IEnumerable<Folder>? Folders { get; set; }
        public IEnumerable<Quiz>? Quizzes { get; set; }
        public IEnumerable<Quiz>? Favourites { get; set; }

        public void OnGet()
        {
            if ((userID = getUserId()) == null) { authErrorPage(); }
            else
            {
                Folders = _libraryService.GetUserFolders(userID);
                Quizzes = _libraryService.GetUserQuizzes(userID);
                var favourites = new List<Quiz>();


                if (Quizzes == null)
                {
                    RedirectToPage("../Error", new { errormessage = "@Library/Index - Could not retrieve quizzes from the db." });
                }
                else
                {
                    foreach (Quiz quiz in Quizzes)
                    {
                        if (quiz.isFavorite)
                        {
                            favourites.Add(quiz);
                        }
                    }
                    Favourites = favourites;
                }
            }

            //Sort quizzes in library by last played (doesn't sort quizzes in favorites tab)
            Quizzes = _libraryService.SortByLastPlayed(Quizzes);
        }


        public IActionResult OnGetToggleFavorite(int id)
        {
            if ((userID = getUserId()) == null) { authErrorPage(); }

            var quiz = _libraryService.GetUserQuiz(id);
            if (quiz.UserId != userID)
            {
                RedirectToPage("../Error", new { errormessage = "@Library/Index(ToggleFavorite) - You do not have access to this quiz." });
            }
            if (quiz != null)
            {
                quiz.isFavorite = !quiz.isFavorite;
                _libraryService.UpdateUserQuiz(quiz);
            }
            else
            {
                RedirectToPage("../Error", new { errormessage = "@Library/Index(ToggleFavorite) - Couldn't fetch quiz." });
            }


            return RedirectToPage("Index");
        }

        //WHERE IS CREATE FOLDER???
        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private RedirectToPageResult authErrorPage()
        {
            return RedirectToPage("../Error", new { errormessage = "You are not logged in (userId = null)" });
        }

    }
}
