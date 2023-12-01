namespace Acuedify.Exceptions
{
    public class EmptyQuestionException : Exception
    {
        public EmptyQuestionException() : base("Question cannot be empty.")
        {
        }
    }
}
