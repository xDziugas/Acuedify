using Acuedify.Models;


namespace Acuedify.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        bool AuthorizeAsync(Quiz quiz);
        bool AuthorizeAsync(Question question);
        //bool AuthorizeAsync(Folder folder);
        String? GetUserId();
    }
}
