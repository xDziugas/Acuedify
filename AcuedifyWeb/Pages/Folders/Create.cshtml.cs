using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Error;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Folders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FolderService _folderService;
        private readonly IAuthService _authService;
        private readonly IErrorService _errorService;

        public CreateModel(FolderService folderService, IAuthService authService, IErrorService errorService)
        {
            _folderService = folderService;
            _authService = authService;
            _errorService = errorService;
        }

        public Folder? Folder { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(Folder folder)
        {
            String? userId = _authService.GetUserId();

            if (ModelState.IsValid)
            {
                folder.UserId = userId;
                await _folderService.CreateFolder(folder);
                return RedirectToPage("/Library/Index");
            }

            return Page();
        }

    }
}
