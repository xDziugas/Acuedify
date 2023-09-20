using Acuedify.Models;

namespace Acuedify.Services.Library.Interfaces
{
    public interface ILibraryService
    {
        List<Quiz> GetUserQuizzes(int userId);
        Quiz GetUserQuiz(int userId, int quizId);
        List<Folder> GetUserFolders(int userid);
        bool CreateUserQuiz(int userId, Quiz quiz, int folderId = 0);
        bool UpdateUserQuiz(int userId, int quizId, Quiz updatedQuiz);
        bool DeleteUserQuiz(int userId, int quizId);
    }
}
