using Acuedify.Data;
using Acuedify.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Acuedify.Services.Library
{
    public class LibraryUtils
    {

        private readonly AppDBContext _dbContext;

        public LibraryUtils(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Quiz>? FilterQuizzes(string query, IEnumerable<Quiz> quizzes)
        {
            try
            {
                string sanitizedQuery = Regex.Escape(query);

                var regex = new Regex(sanitizedQuery, RegexOptions.IgnoreCase);

                var filteredQuizzes = quizzes.Where(q => regex.IsMatch(q.Title));

                return filteredQuizzes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public IEnumerable<Quiz>? FilterPublicQuizzes(string query)
        {
            try
            {
                string sanitizedQuery = Regex.Escape(query);

                var regex = new Regex(sanitizedQuery, RegexOptions.IgnoreCase);

                var filteredQuizzes = 
                    _dbContext.Quizzes?
                    .Where(s => s.AccessLevel == AccessLevel.PUBLIC)
                    .ToList()
                    .Where(q => regex.IsMatch(q.Title)) ?? new List<Quiz>();

                return filteredQuizzes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }
    }
}
