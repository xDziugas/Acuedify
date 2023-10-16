using Acuedify.Models;

namespace Acuedify.Services.Library.Interfaces
{
	public interface ILibraryService
	{
		List<Quiz> GetUserQuizzes(String userId);
		Quiz GetUserQuiz(int quizId, String userId);
		List<Folder> GetUserFolders(String userId);
		bool CreateUserQuiz(Quiz quiz, String userId);
		bool UpdateUserQuiz(Quiz updatedQuiz, String userId);
		bool DeleteUserQuiz(int quizId, String userId);
		List<Question> GetQuizQuestions(int quizId, String userId);
	}
}
