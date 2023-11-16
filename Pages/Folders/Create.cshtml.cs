using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Folders
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly FolderService _folderService;
        private readonly IAuthService _authService;

        public CreateModel(FolderService folderService, IAuthService authService)
        {
            _folderService = folderService;
            _authService = authService; 
        }

        public Folder? Folder { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(Folder folder)
        {
            String? userId = _authService.GetUserId(); 
            if (userId == null) {}
            if (ModelState.IsValid)
            {
                folder.UserId = userId
                _folderService.CreateFolder(folder);
                return RedirectToPage("../Library/Index");
            }

            return Page();
        }

    }
}
