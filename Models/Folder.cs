using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public List<Quiz>? Quizzes { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [NotMapped]
        public User User { get; set; } = null!;

        private bool isFavorite { get; set; } = false;
	}
}
