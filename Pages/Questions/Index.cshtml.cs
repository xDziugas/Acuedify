using Acuedify.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Pages.Questions
{
    public class IndexModel : PageModel
    {
		private readonly AppDBContext _context;

		public IndexModel(AppDBContext context)
		{
			_context = context;
		}
		public void OnGet()
        {

        }
    }
}
