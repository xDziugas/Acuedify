using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Acuedify.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public string message { get; set; } 
        public string address { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
            address = "[address]";
            message = "[error]";
        }

        public void OnGet(String url, String errormessage)
        {
            address = url;
            message = errormessage;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}