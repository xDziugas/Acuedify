namespace Acuedify.Models
{
	public class LibraryDetails
	{
		public IEnumerable<Folder> Folders { get; set; }
		public IEnumerable<Quiz> Quizzes { get; set; }
		public IEnumerable<Quiz> Favourites { get; set; }

		public LibraryDetails(IEnumerable<Folder> folders, IEnumerable<Quiz> quizzes, IEnumerable<Quiz> favourites)
		{
			Folders = folders;
			Quizzes = quizzes;
			Favourites = favourites;
		}
	}
}
