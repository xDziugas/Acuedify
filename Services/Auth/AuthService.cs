using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using System.Security.Claims;

namespace Acuedify.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        String? getUserId()
        {
            return _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }

        bool IAuthService.AuthorizeAsync(Quiz quiz)
        {
            var userId = getUserId();
            if (userId == null || quiz.UserId == userId) { return false; }
            else { return true; }
        }

        bool IAuthService.AuthorizeAsync(Question question)
        {
            var userId = getUserId();
            if (userId == null || question.UserId == userId) { return false; }
            else { return true; }
        }

        //Also uncomment in interface
/*
        bool IAuthService.AuthorizeAsync(Folder folder)
        {
            var userId = getUserId();
            if (userId == null || folder.UserId == userId) { return false; }
            else { return true; }
        }
*/

    }
}
