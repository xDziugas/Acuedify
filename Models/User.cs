using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acuedify.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public ICollection<Quiz>? Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Folder>? Folders { get; set; } = new List<Folder>();
	}
}
