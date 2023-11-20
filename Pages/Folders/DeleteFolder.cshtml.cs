using Acuedify.Models;
using Acuedify.Services.Folders;
using Acuedify.Services.Library;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Folders
{
    [ValidateAntiForgeryToken]
    [Authorize]
    public class DeleteFolderModel : PageModel
    {
        private readonly FolderService _folderService;

        public DeleteFolderModel(FolderService folderService)
        {
            _folderService = folderService;
        }
        public Folder? Folder { get; set; }

        public IActionResult OnGet(int folderId)
        {

            Folder = _folderService.FindFolder(folderId);

            if (Folder == null)
            {
                return NotFound();
            }
            if (Folder.UserId != getUserId())
            {
                return Forbid();
            }

            return Page();
        }


        public IActionResult OnPostConfirm(int folderId)
        {
            var folderToDelete = _folderService.FindFolder(folderId);


            if (folderToDelete == null)
            {
                return NotFound();
            }
            if (folderToDelete.UserId != getUserId())
            {
                return Forbid();
            }

            _folderService.DeleteFolder(folderToDelete);

            return RedirectToPage("../Library/Index");
        }





        //auth helper functions
        private string? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
