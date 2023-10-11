using Microsoft.EntityFrameworkCore;
using Acuedify.Models;

namespace Acuedify.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{
		}

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Question> Question { get; set; } = default!;
	}
}
