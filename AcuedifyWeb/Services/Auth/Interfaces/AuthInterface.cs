using Acuedify.Models;


namespace Acuedify.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        bool Authorized(Quiz quiz);
        bool Authorized(Question question);
        bool Authorized(Folder folder);
        String? GetUserId();
    }
}
