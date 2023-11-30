using Newtonsoft.Json;
using System.Collections;
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

        public AccessLevel AccessLevel { get; set; } = AccessLevel.PRIVATE;

        public int TimesSolved { get; set; } = 0;

        public DateTime? LastPlayed { get; set; } = null;

        //serialized list, so no new tables are created
        public string? PastScoresSerialized { get; set; }

        [NotMapped]
        public List<int> PastScores
        {
            get
            {
                return string.IsNullOrEmpty(PastScoresSerialized)
                    ? new List<int>()
                    : JsonConvert.DeserializeObject<List<int>>(PastScoresSerialized);
            }
            set
            {
                PastScoresSerialized = JsonConvert.SerializeObject(value);
            }
        }
    }
    public enum AccessLevel
    {
        PRIVATE,
        UNLISTED,
        PUBLIC
    }
}
