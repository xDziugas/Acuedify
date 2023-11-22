using Acuedify.Data;
using Acuedify.Models;
using Acuedify.Services.Folders;
using Acuedify.Tests.TestUtilities;

namespace Acuedify.Tests.UnitTests.Services.Folders
{
    public class FolderServiceTests
    {

        private readonly FolderService _folderService;
        private readonly AppDBContext _dbContext;

        public FolderServiceTests() 
        {
            _dbContext = TestUtils.CreateInMemoryDbContext();
            _folderService = new FolderService(_dbContext);
        }

        [Fact]
        public async void CreateFolder__InvokedWithNewFolder__InsertedIntoDbContext()
        {
            var expectedFolder = new Folder
            {
                Name = "Folder Name",
                UserId = "User 1"
            };

            await _folderService.CreateFolder(expectedFolder);

            var actualFolder = _dbContext.Folders.Find(expectedFolder.Id);
            Assert.Equal(expectedFolder, actualFolder);
        }

        [Fact]
        public async void UpdateFolder__InvokedWithUpdatedFolderName__NameIsUpdated()
        {
            seedDefaultFoldersData(_dbContext);
            var folderId = 1;
            var updatedFolder = _dbContext.Folders.Find(folderId);
            updatedFolder.Name = "New Folder Name";

            await _folderService.UpdateFolder(updatedFolder);

            var actualFolder = _dbContext.Folders.Find(folderId);
            Assert.Equal("New Folder Name", actualFolder?.Name);
        }

        [Fact]
        public async void FindFolder__RequestByCorrectId__RetrievesFolderWithQuizzes()
        {
            seedDefaultFoldersData(_dbContext);
            var folderId = 1;
            var expectedFolderName = "Folder 1 Name";
            var expectedQuizCount = 1;

            var actualFolder = await _folderService.FindFolder(folderId);

            Assert.Equal(expectedFolderName, actualFolder?.Name);
            Assert.NotNull(actualFolder?.Quizzes);
            Assert.Equal(expectedQuizCount, actualFolder?.Quizzes.Count);
        }

        [Fact]
        public async void FindUserFordels__RequestByUserId__RetrievesOnlyUserFolders()
        {
            seedDefaultFoldersData(_dbContext);
            var userId = "user_id";
            var expectedFolderCount = 2;
            var folderName1 = "Folder 1 Name";
            var folderName2 = "Folder 2 Name";

            var userFolders = await _folderService.FindUserFolders(userId);

            Assert.Equal(expectedFolderCount, userFolders.Count);
            Assert.Contains(userFolders, folder => folder.Name == folderName1 && folder.Quizzes != null && folder.Quizzes.Count == 1);
            Assert.Contains(userFolders, folder => folder.Name == folderName2 && folder.Quizzes != null && folder.Quizzes.Count == 0);
        }

        [Fact]
        public async void DeleteFolder__RequestToDeleteFolder__FolderIsDeletedAndQuizzesLeftWithoutFolder()
        {
            var folderId = 1;
            _dbContext.Add(new Folder
                {
                    Id = folderId,
                    Name = "Folder 1 Name",
                    UserId = "user_id",
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Id = 1,
                            Title = "Quiz 1",
                            Description = "Quiz 1 description",
                            Questions = new List<Question>()
                        }
                    }
                });
            _dbContext.SaveChanges();

            var folderToBeDeleted = _dbContext.Folders.Find(folderId);
            var folderQuiz = _dbContext.Quizzes.First(quiz => quiz.FolderId == folderId);

            await _folderService.DeleteFolder(folderToBeDeleted);

            var deletedFolder = _dbContext.Folders.Find(folderId);
            Assert.Null(deletedFolder);
            Assert.Null(folderQuiz.FolderId);
        }
        

        private void seedDefaultFoldersData(AppDBContext dbContext)
        {
            dbContext.AddRange(new Folder[]
            {
                new Folder
                {
                    Id = 1,
                    Name = "Folder 1 Name",
                    UserId = "user_id",
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Id = 1,
                            Title = "Quiz 1",
                            Description = "Quiz 1 description",
                            Questions = new List<Question>()
                        }
                    }
                },
                new Folder
                {
                    Id = 2,
                    Name = "Folder 2 Name",
                    UserId = "user_id",
                    Quizzes = new List<Quiz>()
                },
                new Folder
                {
                    Id = 3,
                    Name = "Folder 2 Name",
                    UserId = "other_user_id",
                    Quizzes = new List<Quiz>
                    {
                        new Quiz
                        {
                            Id = 2,
                            Title = "Quiz 2",
                            Description = "Quiz 2 description",
                            Questions = new List<Question>()
                        }
                    }
                },
            });

            dbContext.SaveChanges();
        }

    }
}