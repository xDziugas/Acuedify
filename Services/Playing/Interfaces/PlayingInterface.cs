using Acuedify.Models;

namespace Acuedify.Services.Playing.Interfaces
{
	public interface IPlayingService
	{
		public PlayDetails InitPlayDetails(Quiz flashcardSet, List<Question> flashcards);
		public PlayDetails GetFromSession(string SessionKey, ISession session);
		public void SetToSession(string SessionKey, PlayDetails details, ISession session);
		public bool isValid(PlayDetails details);
		public List<Question> ShuffleFlashcards(List<Question> flashcards);
	}
}
