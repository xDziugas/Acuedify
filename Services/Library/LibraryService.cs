using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Windows;

namespace Acuedify.Services.Library
{
	public class LibraryService : ILibraryService
	{
		private readonly AppDBContext _dbContext;

		public LibraryService(AppDBContext dbContext)
		{
			_dbContext = dbContext;
		}

        Quiz? ILibraryService.GetUserQuiz(int quizId, String userId)
        {
            var currentQuiz = _dbContext.Quizzes?
                .FirstOrDefault(quiz => quiz.Id == quizId && quiz.UserId == userId);

            return currentQuiz;
        }


        List<Quiz> ILibraryService.GetUserQuizzes(String? userId)
        {
            return _dbContext.Quizzes?
                .Where(s => s.UserId == userId)
                .ToList() ?? new List<Quiz>();
        }

        List<Folder> ILibraryService.GetUserFolders(String? userId)
        {
            return _dbContext.Folders?
				.Where(f => f.UserId == userId)
				.ToList() ?? new List<Folder>();
        }

        bool ILibraryService.CreateUserQuiz(Quiz quiz, String userId) //create in folder - add later
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

        bool ILibraryService.UpdateUserQuiz(Quiz updatedQuiz, String userId)
        {
            try
            {


                var currentQuiz = _dbContext.Quizzes?
					.FirstOrDefault(quiz => quiz.Id == updatedQuiz.Id && quiz.UserId == userId);

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

        bool ILibraryService.DeleteUserQuiz(int quizId, String userId)
		{
			try
			{
				var quizToDelete = _dbContext.Quizzes
					.FirstOrDefault(quiz => quiz.Id == quizId && quiz.UserId == userId);

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

		List<Question> ILibraryService.GetQuizQuestions(int quizId, String userId)
		{
			var questions = _dbContext.Quizzes
				.Where(quiz => quiz.Id == quizId)
				.Where(quiz => quiz.UserId == userId)
				.SelectMany(quiz => quiz.Questions)
				.ToList();

			return questions;
		}
    }
}
