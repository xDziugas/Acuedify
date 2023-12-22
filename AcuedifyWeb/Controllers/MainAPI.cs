using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acuedify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAPI : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public MainAPI(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public List<Quiz> Get()
        {
            List<Quiz>? Quizzes = _libraryService.GetPublicQuizzes(30);
            return Quizzes;
        }
    }
}
