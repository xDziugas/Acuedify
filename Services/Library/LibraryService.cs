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

        bool ILibraryService.CreateUserQuiz(int userId, Quiz quiz, int folderId)
        {
            try
            {
                _dbContext.User
                    .FirstOrDefault(user => user.Id == userId)
                    ?.Quizzes
                    ?.Add(quiz);

                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool ILibraryService.DeleteUserQuiz(int userId, int quizId)
        {
            try
            {
                var quizToDelete = _dbContext.User
						.Include(user => user.Quizzes)
						.FirstOrDefault(user => user.Id == userId)
						?.Quizzes
						?.FirstOrDefault(quiz => quiz.Id == quizId);

				if (quizToDelete == null)
                {
                    return false;
                }

                _dbContext.User
                    .FirstOrDefault(user => user.Id == userId)
                    ?.Quizzes
                    ?.Remove(quizToDelete);

                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        Quiz ILibraryService.GetUserQuiz(int userId, int quizId)
        {
            var currentQuiz = _dbContext.User
                        .Include(user => user.Quizzes)
                        .FirstOrDefault(user => user.Id == userId)
                        ?.Quizzes
                        ?.FirstOrDefault(quiz => quiz.Id == quizId);

            return currentQuiz;
        }

        List<Quiz> ILibraryService.GetUserQuizzes(int userId)
        {
            return _dbContext.User
                .Include(user => user.Quizzes)
                .FirstOrDefault(user => user.Id == userId)
                ?.Quizzes?.ToList() ?? new List<Quiz>();
        }

        bool ILibraryService.UpdateUserQuiz(int userId, int quizId, Quiz updatedQuiz)
        {
            try
            {
                var currentQuiz = _dbContext.User
                    .FirstOrDefault(user => user.Id == userId)
                    ?.Quizzes
                    ?.FirstOrDefault(quiz => quiz.Id == quizId);

                if (currentQuiz == null)
                    return false;

                currentQuiz = updatedQuiz;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        List<Folder> ILibraryService.GetUserFolders(int userId)
        {
            return _dbContext.User
                .Include(user => user.Folders)
                .FirstOrDefault(user => user.Id == userId)
                ?.Folders?.ToList() ?? new List<Folder>();
        }

    }
}
