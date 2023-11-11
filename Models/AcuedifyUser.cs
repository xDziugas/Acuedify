using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class AcudefyUser : IdentityUser
    {
        [ForeignKey("UserID")]
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();

        [ForeignKey("UserId")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}