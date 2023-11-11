using Acuedify.Data;
using Acuedify.Services.Questions.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Services.Questions
{
    public class QuestionsService : IQuestionsService
    {
        private readonly AppDBContext _dbContext;

        public QuestionsService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        List<SelectListItem> IQuestionsService.GetQuizIdsAsSelectListItems(int selectQuizId, string userID)
        {
            return _dbContext.Quizzes
                .Where(quiz => quiz.UserId == userID)
                .Select(q => q.Id)
                .Select(id => new SelectListItem(id.ToString(), id.ToString(), id == selectQuizId))
                .ToList();
        }
        bool IQuestionsService.QuestionExists(int id)
        {
            return (_dbContext.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
