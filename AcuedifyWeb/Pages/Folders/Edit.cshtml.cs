using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static NuGet.Packaging.PackagingConstants;

namespace Acuedify.Pages.Folders
{
	[Authorize]
    public class EditModel : PageModel
    {
        private readonly FolderService _folderService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public EditModel(FolderService folderSerivce,
            IAuthService authService, IErrorService errorService)
        {
            _folderService = folderSerivce;
            _authService = authService;
            _errorService = errorService;
        }

        public Folder? Folder { get; set; }

        public async Task<IActionResult> OnGet(int folderId)
        {
            String? userId = _authService.GetUserId();

            Folder = await _folderService.FindFolder(folderId);

            if (Folder == null)
            {
                return _errorService.ErrorPage(this, "folder not found");
            }

            //authorization check
            if ( ! _authService.Authorized(Folder) ) { return Forbid(); }

            return Page();
        }


        public async Task<IActionResult> OnPost(Folder folder)
        {
            String? userId = _authService.GetUserId();

            //authorization check
            if ( ! _authService.Authorized(folder) ) { return Forbid(); } 


            if (ModelState.IsValid)
            {
                await _folderService.UpdateFolder(folder);
            }

            Folder = folder;
            return Page();
        }
    }
}
