using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Folders;
using Acuedify.Services.Library;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Acuedify.Pages.Public
{
    public class IndexModel : PageModel
    {
        private readonly ILibraryService _libraryService;
        private readonly LibraryUtils _libraryUtils;

        public IndexModel(ILibraryService libraryService,
          FolderService folderService,
          LibraryUtils libraryUtils, IMemoryCache cache,
          IAuthService authService, IErrorService errorService)
        {
            _libraryService = libraryService;
            _libraryUtils = libraryUtils;
        }

        public IEnumerable<Quiz>? Quizzes { get; set; }
        public IEnumerable<Quiz>? filteredQuizzes { get; set; }

        public async Task OnGet()
        {
            Quizzes = _libraryService.GetPublicQuizzes(30);

            //Sort quizzes in library by last played (doesn't sort quizzes in favorites tab)
            //Quizzes = _libraryService.SortByLastPlayed(Quizzes);
        }




        public IActionResult OnGetSearch(string query)
        {
            System.Diagnostics.Debug.WriteLine(query);
            filteredQuizzes = _libraryUtils.FilterPublicQuizzes(query);
            System.Diagnostics.Debug.WriteLine(filteredQuizzes);


            return new PartialViewResult
            {
                ViewName = "_SearchResults",
                ViewData = new ViewDataDictionary<IndexModel>(ViewData, this)
            };
        }

    }
}