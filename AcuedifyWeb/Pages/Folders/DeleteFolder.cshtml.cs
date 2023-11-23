using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Folders
{
    [ValidateAntiForgeryToken]
    [Authorize]
    public class DeleteFolderModel : PageModel
    {
        private readonly FolderService _folderService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public DeleteFolderModel(FolderService folderService,
            IAuthService authService, IErrorService errorService)
        {
            _folderService = folderService;
            _authService = authService;
            _errorService = errorService;
        }

        public Folder? Folder { get; set; }

        public async Task<IActionResult> OnGet(int folderId)
        {

            Folder = await _folderService.FindFolder(folderId);

            if (Folder == null)
            {
                return _errorService.ErrorPage(this, "folder not found");
            }

            //authorization check
            if ( ! _authService.Authorized(Folder) ) { return Forbid(); }

            return Page();
        }


        public async Task<IActionResult> OnPostConfirm(int folderId)
        {
            var folderToDelete = await _folderService.FindFolder(folderId);


            if (folderToDelete == null)
            {
                return _errorService.ErrorPage(this, "folder to delete not found");
            }

            //authorization check
            if ( ! _authService.Authorized(folderToDelete) ) { return Forbid(); }

            await _folderService.DeleteFolder(folderToDelete);

            return RedirectToPage("/Library/Index");
        }
    }
}
