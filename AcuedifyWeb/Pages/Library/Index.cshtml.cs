using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Folders;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Library
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        private readonly FolderService _folderService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public IndexModel(ILibraryService libraryService,
            FolderService folderService,
            IAuthService authService, IErrorService errorService)
        {
            _libraryService = libraryService;
            _folderService = folderService;
            _authService = authService;
            _errorService = errorService;
        }

        public IEnumerable<Folder>? Folders { get; set; }
        public IEnumerable<Quiz>? Quizzes { get; set; }
        public IEnumerable<Quiz>? Favourites { get; set; }

        public async Task OnGet()
        {
            String? userId = _authService.GetUserId();

            Folders = await _folderService.FindUserFolders(userId);
            Quizzes = _libraryService.GetUserQuizzes(userId);
            var favourites = new List<Quiz>();

            if (Quizzes == null)
            {
                _errorService.ErrorPage(this, "quizzes not found");
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

            //Sort quizzes in library by last played (doesn't sort quizzes in favorites tab)
            Quizzes = _libraryService.SortByLastPlayed(Quizzes);
        }


        public IActionResult OnGetToggleFavorite(int id)
        {
            var quiz = _libraryService.GetUserQuiz(id);

            //authorization check
            if (!_authService.Authorized(quiz)) { return Forbid(); }


            if (quiz == null)
            {
                return _errorService.ErrorPage(this, "quiz not found");
            }

            else
            {
                quiz.isFavorite = !quiz.isFavorite;
                _libraryService.UpdateUserQuiz(quiz);
            }


            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetToggleFolderChange(int quizId, int? newFolderId)
        {
            var quiz = _libraryService.GetUserQuiz(quizId);


            if (quiz == null)
            {
                return _errorService.ErrorPage(this, "quiz not found");
            }

            //quiz authorization check
            if (!_authService.Authorized(quiz)) { return Forbid(); }

            if (newFolderId != null)
            {
                var folder = await _folderService.FindFolder(newFolderId.Value);
                if (folder == null)
                {
                    return _errorService.ErrorPage(this, "folder not found");
                }
                //folder authorization check
                if (!_authService.Authorized(folder)) { return Forbid(); }
            }

            quiz.FolderId = newFolderId;
            _libraryService.UpdateUserQuiz(quiz);

            return RedirectToPage("Index");
        }
    }
}
