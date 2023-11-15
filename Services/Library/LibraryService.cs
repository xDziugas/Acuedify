using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Library.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public Folder? GetFolder(int folderId)
        {
			return _dbContext.Folders?
				.Where(folder => folder.Id == folderId)
                .Include(folder => folder.Quizzes)
                .FirstOrDefault();
        }

        public bool DeleteFolder(int folderId)
        {
            try
            {
				var folderToDelete = GetFolder(folderId);

                if (folderToDelete == null)
                {
                    return false;
                }

				foreach (var quiz in folderToDelete.Quizzes)
				{
					quiz.FolderId = null;
					_dbContext.Quizzes.Update(quiz);
				}

                _dbContext.Folders
                    .Remove(folderToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        List<Question> ILibraryService.GetQuizQuestions(int quizId)
		{
			var questions = _dbContext.Quizzes
				.Where(quiz => quiz.Id == quizId).SelectMany(quiz => quiz.Questions).ToList();

			return questions;
		}
	}
}
