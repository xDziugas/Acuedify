using Acuedify.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Acuedify.Services.Library
{
    public class LibraryUtils
    {
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
    }
}
