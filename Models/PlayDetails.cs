namespace Acuedify.Models
{
    public class PlayDetails
    {
        public int CurrentIndex { get; set; } = 0;
        public Quiz? Quiz { get; set; } = null;
        public int CorrectAnswers { get; set; } = 0;
        public int IncorrectAnswers { get; set; } = 0;
    }
}
