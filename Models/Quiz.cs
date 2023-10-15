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
        public List<Question> Questions { get; set; } = new List<Question>();
        public bool isFavorite { get; set; } = false;

        [ForeignKey("Folder")]
        public int? FolderId { get; set; } = null;

        public Folder? Folder { get; set; } = null;

        public string? UserId { get; set; } = null;

    }
}
