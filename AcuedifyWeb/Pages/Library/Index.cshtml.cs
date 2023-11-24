using Acuedify.Models;
using Acuedify.Services.Folders;
using Acuedify.Services.Library;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Acuedify.Pages.Library
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        private readonly FolderService _folderService;
        private readonly UserManager<AcuedifyUser> _userManager;
        private readonly LibraryUtils _libraryUtils;
        private readonly IMemoryCache _cache;
        private string? userID;

        public IndexModel(ILibraryService libraryService, FolderService folderService, UserManager<AcuedifyUser> userManager, LibraryUtils libraryUtils, IMemoryCache cache)
        {
            _libraryService = libraryService;
            _folderService = folderService;
            _userManager = userManager;
            _libraryUtils = libraryUtils;
            _cache = cache;
        }

        public IEnumerable<Folder>? Folders { get; set; }
        public IEnumerable<Quiz>? Quizzes { get; set; }
        public IEnumerable<Quiz>? Favourites { get; set; }
        public IEnumerable<Quiz>? filteredQuizzes { get; set; }

        public async Task OnGet()
        {
            if ((userID = getUserId()) == null) { authErrorPage(); }

            Folders = await _folderService.FindUserFolders(userID);
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

            //Sort quizzes in library by last played (doesn't sort quizzes in favorites tab)
            Quizzes = _libraryService.SortByLastPlayed(Quizzes);

            if (!_cache.TryGetValue(Constants.LibrarySessionKey, out IEnumerable<Quiz> quizzes))
            {
                //keep in cache for 5 minutes if not accessed
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                // Save the data in cache
                _cache.Set(Constants.LibrarySessionKey, Quizzes, cacheEntryOptions);
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

        public async Task<IActionResult> OnGetToggleFolderChange(int quizId, int? newFolderId)
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
                var folder = await _folderService.FindFolder(newFolderId.Value);
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

        public IActionResult OnGetSearch(string query)
        {
            _cache.TryGetValue(Constants.LibrarySessionKey, out IEnumerable<Quiz> cachedQuizzes);

            filteredQuizzes = _libraryUtils.FilterQuizzes(query, cachedQuizzes ?? Enumerable.Empty<Quiz>());

            return new PartialViewResult
            {
                ViewName = "_SearchResults",
                ViewData = new ViewDataDictionary<IndexModel>(ViewData, this)
            };
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
