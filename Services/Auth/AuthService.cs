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
            //System.Diagnostics.Trace.WriteLine(string.Join(Environment.NewLine, _httpContextAccessor.HttpContext.GetRouteData().Values));
            
            return _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

        }

        bool IAuthService.AuthorizeAsync(Quiz quiz)
        {
            var userId = ((IAuthService)this).GetUserId();
            if (userId == null || quiz.UserId == userId) { return false; }
            else { return true; }
        }

        bool IAuthService.AuthorizeAsync(Question question)
        {
            var userId = ((IAuthService)this).GetUserId();
            if (userId == null || question.UserId == userId) { return false; }
            else { return true; }
        }

        //Also uncomment in interface
        /*
                bool IAuthService.AuthorizeAsync(Folder folder)
                {
                    var userId = ((IAuthService)this).GetUserId();
                    if (userId == null || folder.UserId == userId) { return false; }
                    else { return true; }
                }
        */

    }
}
