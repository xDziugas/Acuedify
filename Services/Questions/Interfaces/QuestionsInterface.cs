using Acuedify.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Services.Questions.Interfaces
{
    public interface IQuestionsService
    {
        List<SelectListItem> GetQuizIdsAsSelectListItems(int selectQuizId, string userID);
        bool QuestionExists(int id);
    }
}
