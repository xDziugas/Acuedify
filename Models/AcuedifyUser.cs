using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class AcuedifyUser : IdentityUser
    {
        //[ForeignKey("UserId")]
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();

        [ForeignKey("UserId")]
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}