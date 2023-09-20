using Microsoft.EntityFrameworkCore;
using Acuedify.Models;

namespace Acuedify.Data
{
	public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
