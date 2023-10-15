using Microsoft.EntityFrameworkCore;
using Acuedify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Acuedify.Data
{
	public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Question> Question { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}