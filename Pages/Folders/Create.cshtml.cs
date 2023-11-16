using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Folders;
using Acuedify.Services.Questions.Interfaces;
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

        public CreateModel(FolderService folderService)
        {
            _folderService = folderService;
        }

        public Folder? Folder { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(Folder folder)
        {
            if (ModelState.IsValid)
            {
                folder.UserId = getUserId();
                _folderService.CreateFolder(folder);
                return RedirectToPage("../Library/Index");
            }

            return Page();
        }

        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
