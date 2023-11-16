using Acuedify.Data;
using Acuedify.Models;
using Microsoft.EntityFrameworkCore;

namespace Acuedify.Services.Folders
{
	public class FolderService
	{
		private readonly AppDBContext _dbContext;

		public FolderService(AppDBContext dbContext) 
		{ 
			_dbContext = dbContext; 
		}

		public void CreateFolder(Folder folder)
		{
			_dbContext.Folders.Add(folder);
			_dbContext.SaveChanges();
		}

		public void UpdateFolder(Folder folder)
		{
			_dbContext.Folders.Update(folder);
			_dbContext.SaveChanges();
		}

		public Folder? FindFolder(int folderId) 
		{ 
			return _dbContext.Folders
				.Include(folder => folder.Quizzes)
                .FirstOrDefault(folder  => folder.Id == folderId);
		}

		public List<Folder> GetUserFolders(string userId) 
		{
			return _dbContext.Folders
				.Include(folder => folder.Quizzes)
				.Where(folder => folder.UserId == userId)
				.ToList();
		}

		public void DeleteFolder(Folder folderToDelete)
		{
			if (folderToDelete == null)
			{
				return;
			}

			foreach (var quiz in folderToDelete.Quizzes)
			{
				quiz.FolderId = null;
				_dbContext.Quizzes.Update(quiz);
			}

			_dbContext.Folders.Remove(folderToDelete);
			_dbContext.SaveChanges();

		}

	}
}
