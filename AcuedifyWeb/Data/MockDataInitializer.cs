using Acuedify.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Acuedify.Data
{
    public class MockDataInitializer
    {


        public static void SeedUsers(AppDBContext context, string filePath)
        {
            /*try
            {
                string listOfUsersJson = File.ReadAllText(filePath);
                var users = JsonSerializer.Deserialize<List<AcuedifyUser>>(listOfUsersJson);
                if (users != null)
                {
                    // WHY DOESNT THIS WORK
                    *//*context.Database.SqlQuery<int>($"DBCC CHECKIDENT ('dbo.Question', RESEED, 0);");
                    context.Database.SqlQuery<int>($"GO");
                    context.Database.SqlQuery<int>($"DBCC CHECKIDENT ('dbo.Quizzes', RESEED, 0);")
                    context.Database.SqlQuery<int>($"GO");*//*
                    context.AddRange(users);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }*/
        }
    }
}