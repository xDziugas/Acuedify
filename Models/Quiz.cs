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
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [NotMapped]
        public User User { get; set; } = null!;
        public DateTime DateOfCreation { get; set; }
        public bool isFavorite { get; set; } = false;

		[ForeignKey("Folder")]
		public int? FolderId { get; set; }

		public Folder? Folder { get; set; }


		//galima daug ko
	}
}
