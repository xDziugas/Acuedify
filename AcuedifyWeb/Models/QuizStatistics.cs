namespace Acuedify.Models
{
    public class QuizStatistics
    {
        public Quiz Quiz { get; set; }
        public List<Question> Questions { get; set; }
        public int CurrentIndex { get; set; } = 0;
        public int TotalSolved { get; set; } = 1;
        public List<double> Weight { get; set; } = new List<double>();
    }
}
