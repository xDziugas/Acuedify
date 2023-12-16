using Acuedify.Models;

namespace Acuedify.Services.Playing.Interfaces
{
	public interface IPlayingService
	{
		public PlayDetails InitPlayDetails(Quiz flashcardSet, List<Question> flashcards);
		public T GetFromSession<T>(string SessionKey, ISession session);
        public void SetToSession<T>(string SessionKey, T details, ISession session);

        public bool isValid(PlayDetails details);
        public bool isValid(QuizStatistics details);

        public List<Question> ShuffleFlashcards(List<Question> flashcards);

		public List<Question> ShuffleByDifficulty(List<Question> flashcards);
        public QuizStatistics InitQuizStatistics(Quiz flashcardSet, List<Question> flashcards);
		public QuizStatistics UpdateWeight(QuizStatistics stats, int questionId, bool isCorrect);

    }
}
