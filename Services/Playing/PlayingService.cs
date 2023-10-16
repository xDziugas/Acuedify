using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Playing.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Acuedify.Services.Playing
{
	public class PlayingService : IPlayingService
	{
		PlayDetails IPlayingService.InitPlayDetails(Quiz flashcardSet, List<Question> flashcards)
		{
			PlayDetails details = new PlayDetails();

			details.Quiz = flashcardSet;
			
			details.Quiz.Questions = flashcards;

			return details;
		}

		PlayDetails IPlayingService.GetFromSession(string SessionKey, ISession session)
		{
			return session.GetObject<PlayDetails>(SessionKey);
		}

		void IPlayingService.SetToSession(string SessionKey, PlayDetails details, ISession session)
		{
			session.SetObject(SessionKey, details);
		}

		String? IPlayingService.isValid(PlayDetails details)
		{
			if (details == null) { return "Couldn't Initialise quiz"; }
			if (details.Quiz == null) { return "Quiz is null"; }
			if (details.CurrentIndex > details.Quiz.Questions.Count - 1) { return "Flashcard index out of range"; }
			return null;
		}

		List<Question> IPlayingService.ShuffleFlashcards(List<Question> flashcards)
		{
			flashcards.Shuffle();

			return flashcards;
		}
	}
}
