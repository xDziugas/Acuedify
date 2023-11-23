using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acuedify.Services.Error.Interfaces
{
    public interface IErrorService
    {
        RedirectToPageResult ErrorPage(PageModel pageModel, String errorerrorMessagestring);
    }
}
