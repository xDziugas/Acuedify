using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [NotMapped]
        public User User { get; set; } = null!;

        public DateTime DateOfCreation { get; set; }
        public bool isFavorite { get; set; } = false;

        //times solved etc galima daug ko
    }
}
