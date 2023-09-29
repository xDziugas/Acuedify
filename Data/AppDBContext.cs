﻿using Microsoft.EntityFrameworkCore;
using Acuedify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Acuedify.Data
{
	public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Folder> Folders { get; set; }
	}
}
