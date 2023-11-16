using Acuedify.Models;
using Acuedify.Services.Folders;
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
		private readonly FolderService _folderService;
		private readonly UserManager<AcuedifyUser> _userManager;
        private string? userID;

        public IndexModel(ILibraryService libraryService, FolderService folderService, UserManager<AcuedifyUser> userManager)
        {
            _libraryService = libraryService;
            _folderService = folderService;
            _userManager = userManager;
        }
                        
        public IEnumerable<Folder>? Folders { get; set; } 
        public IEnumerable<Quiz>? Quizzes { get; set; }
		public IEnumerable<Quiz>? Favourites { get; set; }

        public void OnGet()
        {
            if ((userID = getUserId()) == null) { authErrorPage(); }
            else
            {
                Folders = _folderService.GetUserFolders(userID);
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

        public IActionResult OnGetToggleFolderChange(int quizId, int? newFolderId)
        {
            var quiz = _libraryService.GetUserQuiz(quizId);

            if (quiz == null)
            {
                return NotFound();
            }
            if (quiz.UserId != getUserId())
            {
                return Forbid();
            }

            if (newFolderId != null)
            {
				var folder = _folderService.FindFolder(newFolderId.Value);
                if (folder == null)
                {
                    return NotFound();
                }
                if (folder.UserId != getUserId())
                {
                    return Forbid();
                }
			}

			quiz.FolderId = newFolderId;
			_libraryService.UpdateUserQuiz(quiz);

			return RedirectToPage("Index");
		}



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
