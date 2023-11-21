using Acuedify.Models;
using Acuedify.Services.Folders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acuedify.Pages.Folders
{
	[Authorize]
    public class EditModel : PageModel
    {
        private readonly FolderService _folderService;

        public EditModel(FolderService folderSerivce)
        {
            _folderService = folderSerivce;
        }

        public Folder? Folder { get; set; }

        public async Task<IActionResult> OnGet(int folderId)
        {

            Folder = await _folderService.FindFolder(folderId);

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


        public async Task<IActionResult> OnPost(Folder folder)
        {   

            if(folder.UserId != getUserId()) 
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                await _folderService.UpdateFolder(folder);
            }

            Folder = folder;
            return Page();
        }





        //auth helper functions
        private String? getUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
