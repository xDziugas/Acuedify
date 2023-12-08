using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Acuedify.Services.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly AppDBContext _dbContext;

        public LibraryService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        bool ILibraryService.CreateUserQuiz(Quiz quiz) //create in folder - add later
        {
            try
            {
                _dbContext.Quizzes.Add(quiz);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        bool ILibraryService.DeleteUserQuiz(int quizId)
        {
            try
            {
                var quizToDelete = _dbContext.Quizzes.FirstOrDefault(quiz => quiz.Id == quizId);

                if (quizToDelete == null)
                {
                    return false;
                }

                _dbContext.Quizzes
                    .Remove(quizToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        Quiz ILibraryService.GetUserQuiz(int quizId)
        {
            var currentQuiz = _dbContext.Quizzes
                ?.FirstOrDefault(quiz => quiz.Id == quizId);

            return currentQuiz;
        }

        List<Quiz> ILibraryService.GetUserQuizzes(String id)
        {
            return _dbContext.Quizzes?
                .Where(s => s.UserId == id)
                .ToList() ?? new List<Quiz>();
        }

        bool ILibraryService.UpdateUserQuiz(Quiz updatedQuiz)
        {
            try
            {
                var currentQuiz = _dbContext.Quizzes
                    ?.FirstOrDefault(quiz => quiz.Id == updatedQuiz.Id);

                if (currentQuiz == null)
                {
                    return false;
                }

                _dbContext.Entry(currentQuiz).CurrentValues.SetValues(updatedQuiz); //?

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        List<Folder> ILibraryService.GetUserFolders(string userId)
        {
            return _dbContext.Folders?
                .Where(folder => folder.UserId == userId)
                .ToList() ?? new List<Folder>();
        }

        List<Question> ILibraryService.GetQuizQuestions(int quizId)
        {
            var questions = _dbContext.Quizzes
                .Where(quiz => quiz.Id == quizId).SelectMany(quiz => quiz.Questions).ToList();

            return questions;
        }

        int? ILibraryService.CalculateAverageScore(int quizId)
        {
            try
            {
                var quiz = _dbContext.Quizzes.FirstOrDefault(q => q.Id == quizId);

                var questions = _dbContext.Quizzes
                .Where(quiz => quiz.Id == quizId).SelectMany(quiz => quiz.Questions).ToList();

                var pastScores = quiz.PastScores;

                if (pastScores.IsNullOrEmpty())
                {
                    return 0;
                }
                else
                {
                    return (int)((pastScores.Average() / questions.Count) * 100);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        bool ILibraryService.UpdateQuizResult(QuizResultsModel result)
        {
            try
            {
                var quiz = _dbContext.Quizzes.FirstOrDefault(q => q.Id == result.quizId);

                var questions = _dbContext.Quizzes
                .Where(quiz => quiz.Id == result.quizId).SelectMany(quiz => quiz.Questions).ToList();

                if (quiz == null)
                {
                    return false;
                }

                quiz.TimesSolved++;

                var pastScores = quiz.PastScores;

                if (pastScores.Count >= 3)
                {
                    pastScores.RemoveAt(0);
                }

                pastScores.Add(result.correct);

                quiz.PastScores = pastScores;

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        List<Quiz> ILibraryService.GetPublicQuizzes(int amount)
        {
            return _dbContext.Quizzes?
                .Where(s => s.AccessLevel == AccessLevel.PUBLIC)
                .OrderBy(s => s.TimesSolved)
                .Take(amount)
                .ToList() ?? new List<Quiz>();
        }



        public bool UpdateProperties(int quizId)
        {
            try
            {
                var quiz = _dbContext.Quizzes.FirstOrDefault(q => q.Id == quizId);
                quiz.LastPlayed = DateTime.UtcNow;
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        IEnumerable<Quiz>? ILibraryService.SortByLastPlayed(IEnumerable<Quiz>? quizzes)
        {
            return quizzes?.OrderByDescending(quiz => quiz.LastPlayed);
        }
    }
}
