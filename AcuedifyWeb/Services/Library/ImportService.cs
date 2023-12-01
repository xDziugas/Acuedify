using Acuedify.Data;
using Acuedify.Exceptions;
using Acuedify.Models;
using Acuedify.Services.Auth.Interfaces;
using Acuedify.Services.Folders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Acuedify.Services.Library
{
    public class ImportService
    {

        private readonly AppDBContext _context;
        private readonly IAuthService _authService;
        private readonly FolderService _folderService;

        public ImportService(AppDBContext context, IAuthService authService, FolderService folderService)
        {
            _context = context;
            _authService = authService;
            _folderService = folderService;
        }

        public async Task importLibrary(string importJsonString)
        {
            if (string.IsNullOrWhiteSpace(importJsonString))
            {
                return;
            }

            var a = await _context.Folders
                .Include(folder => folder.Quizzes)
                    .ThenInclude(quiz => quiz.Questions)
                 .Where(folder => folder.UserId == "624650fc-05aa-49b9-9b06-9b1f844c13cb")
                 .ToListAsync();
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string jsonString = JsonSerializer.Serialize(a, options);


            var foldersToImport = JsonSerializer.Deserialize<List<Folder>>(importJsonString);
            if (foldersToImport == null) 
            {
                return;
            }

            validateEachQuestion(foldersToImport, validateQuestion);
            setUserIdToResources(foldersToImport);

            await _context.Folders.AddRangeAsync(foldersToImport);
            _context.SaveChanges();

            Console.WriteLine(foldersToImport);

        }


        private void validateEachQuestion(List<Folder> folders, Action<Question> questionValidationDelegate)
        {
            foreach (var folder in folders)
            {
                foreach (var quiz in folder.Quizzes)
                {
                    foreach (var question in quiz.Questions)
                    {
                        questionValidationDelegate(question);
                    }
                }
            }
        }

        private void validateQuestion(Question question)
        {
            if (question.Term == null || question.Definition == null)
            {
                throw new EmptyQuestionException();
            }
        }

        private void setUserIdToResources(List<Folder> folders)
        {
            String? userId = _authService.GetUserId();
            if (userId == null)
            {
                return;
            }

            foreach (var folder in folders)
            {
                folder.UserId = userId;
                foreach (var quiz in folder.Quizzes)
                {
                    quiz.UserId = userId;
                    foreach (var question in quiz.Questions)
                    {
                        question.UserId = userId;
                    }
                }
            }
        }

    }
}
