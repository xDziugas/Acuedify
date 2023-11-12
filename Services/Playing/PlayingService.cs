using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Playing.Interfaces;
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

        bool IPlayingService.isValid(PlayDetails details)
        {
            if (details == null || details.Quiz == null)
            {
                return false;
            }

            return true;
        }

        List<Question> IPlayingService.ShuffleFlashcards(List<Question> flashcards)
        {
            flashcards.Shuffle();

            return flashcards;
        }


    }
}
