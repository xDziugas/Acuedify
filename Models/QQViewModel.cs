using System.ComponentModel;

namespace Acuedify.Models
{
    public class QQViewModel
    {
        [DisplayName("Quiz")]
        public Quiz Quiz { get; set; }

        [DisplayName("Question")]
        public Question Question { get; set; }
    }
}