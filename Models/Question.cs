using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Term { get; set; }
        [Required]
        public string? Definition { get; set; }
        [ForeignKey("QuizId")]
        public int QuizId { get; set; }
        public string? UserId { get; set; } = null;
        public Difficulty Difficulty { get; set; } = 0;
    }

    public enum Difficulty
    {
        EASY = 1,
        MEDIUM,
        HARD
    }
}
