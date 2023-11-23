using Acuedify.Services.Error.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Acuedify.Services.Error
{
    public class ErrorService : IErrorService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public ErrorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        RedirectToPageResult IErrorService.ErrorPage(PageModel pageModel, String errorMessage)
        {
            String? address = _httpContextAccessor
                .HttpContext
                .GetRouteData()
                .Values["page"]
                .ToString();


            return pageModel.RedirectToPage("/Error", 
                new { url = address, errormessage = errorMessage });
        }
    }
}
