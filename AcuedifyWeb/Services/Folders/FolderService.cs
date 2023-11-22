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

		public async Task CreateFolder(Folder folder)
		{
			_dbContext.Folders.Add(folder);
			await _dbContext.SaveChangesAsync();
		}

		public async Task UpdateFolder(Folder folder)
		{
			_dbContext.Folders.Update(folder);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Folder?> FindFolder(int folderId) 
		{ 
			return await _dbContext.Folders
				.Include(folder => folder.Quizzes)
                .FirstOrDefaultAsync(folder  => folder.Id == folderId);
		}

		public async Task<List<Folder>> FindUserFolders(string userId) 
		{
			return await _dbContext.Folders
				.Include(folder => folder.Quizzes)
				.Where(folder => folder.UserId == userId)
				.ToListAsync();
		}

		public async Task DeleteFolder(Folder folderToDelete)
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
			await _dbContext.SaveChangesAsync();
		}

	}
}
