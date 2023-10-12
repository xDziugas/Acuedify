using Acuedify.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace Acuedify.Data
{
    public class MockDataInitializer
    {
        public static void SeedQuizzes(AppDBContext context, string filePath)
        {
            try
            {
                string listOfQuizzesJson = File.ReadAllText(filePath);
                var quizzes = JsonSerializer.Deserialize<List<Quiz>>(listOfQuizzesJson);
                if (quizzes != null)
                {
                    context.AddRange(quizzes);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
