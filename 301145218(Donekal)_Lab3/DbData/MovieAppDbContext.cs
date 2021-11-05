using _301145218_Donekal__Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301145218_Donekal__Lab3.DbData
{
    public class MovieAppDbContext: DbContext
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.UseSqlServer($"");
            base.OnConfiguring(optionsBuilder);
        }

    }
}
