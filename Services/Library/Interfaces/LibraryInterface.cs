using Acuedify.Models;

namespace Acuedify.Services.Library.Interfaces
{
	public interface ILibraryService
	{
		List<Quiz> GetUserQuizzes(string userId);
		Quiz GetUserQuiz(int quizId);
		List<Folder> GetUserFolders(string userId);
		bool CreateUserQuiz(Quiz quiz);
		bool UpdateUserQuiz(Quiz updatedQuiz);
		bool DeleteUserQuiz(int quizId);
		List<Question> GetQuizQuestions(int quizId);
	}
}
