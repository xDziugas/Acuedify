using Acuedify.Models;
using Acuedify.Services.Playing.Interfaces;
using Microsoft.AspNetCore.Components;

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

        T IPlayingService.GetFromSession<T>(string SessionKey, ISession session)
        {
            return session.GetObject<T>(SessionKey);
        }

        void IPlayingService.SetToSession<T>(string SessionKey, T details, ISession session)
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

        bool IPlayingService.isValid(QuizStatistics details)
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

        List<Question> IPlayingService.ShuffleByDifficulty(List<Question> flashcards)
        {
            return flashcards.OrderByDescending(question => question.Difficulty).ToList();
        }

        QuizStatistics IPlayingService.InitQuizStatistics(Quiz flashcardSet, List<Question> flashcards)
        {
            QuizStatistics stats = new QuizStatistics();
            stats.Questions = flashcards;
            stats.Quiz = flashcardSet;

            foreach (Question question in flashcards)
            {
                stats.Weight.Add(1.0);
            }

            return stats;
        }

        QuizStatistics IPlayingService.UpdateWeight(QuizStatistics stats, int questionId, bool isCorrect)
        {
            //todo: add constants?
            if (isCorrect)
            {
                stats.Weight[questionId] *= 0.9;
            }
            else
            {
                stats.Weight[questionId] *= 1.1;
            }

            if (stats.Weight[questionId] < 0.1)
            {
                stats.Weight[questionId] = 0.1;
            }

            return stats;
        }
    }
}
