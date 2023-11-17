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
        
        String? IAuthService.GetUserId()
        {
            
            return _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

        }

        bool IAuthService.Authorized(Quiz quiz)
        {
            var userId = ((IAuthService)this).GetUserId();
/*
            System.Diagnostics.Trace.WriteLine("user id: " + userId);
            System.Diagnostics.Trace.WriteLine("quiz user id: " + quiz.UserId);
*/
            if (userId == null || quiz.UserId != userId) { return false; }
            else { return true; }
        }

        bool IAuthService.Authorized(Question question)
        {
            var userId = ((IAuthService)this).GetUserId();
            if (userId == null || question.UserId != userId) { return false; }
            else { return true; }
        }

        bool IAuthService.Authorized(Folder folder)
        {
            var userId = ((IAuthService)this).GetUserId();
            if (userId == null || folder.UserId != userId) { return false; }
            else { return true; }
        }


    }
}
