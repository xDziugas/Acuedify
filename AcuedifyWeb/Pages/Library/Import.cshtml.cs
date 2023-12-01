using Acuedify.Exceptions;
using Acuedify.Services.Error.Interfaces;
using Acuedify.Services.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Acuedify.Pages.Library
{
    public class ImportModel : PageModel
    {

        private readonly ImportService _importService;
        private readonly IErrorService _errorService;

        public ImportModel(ImportService importService, IErrorService errorService)
        {
            _importService = importService;
            _errorService = errorService;
        }

        [BindProperty]
        public string? InputString { get; set; }

        public async Task<IActionResult>  OnPost()
        {
            try
            {
                await _importService.importLibrary(InputString);
            }
            catch (JsonException)
            {
                return _errorService.ErrorPage(this, "Given json for importing is incorrect");
            }
            catch (EmptyQuestionException)
            {
                return _errorService.ErrorPage(this, "Question must have term and definition. It cannot be empty.");
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToPage("Index");
        }

    }
}
