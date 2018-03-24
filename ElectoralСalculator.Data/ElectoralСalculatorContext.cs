using ElectoralСalculator.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralСalculator.Data
{
    public class ElectoralСalculatorContext : DbContext
    {
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Result> Results { get; set; }

        public ElectoralСalculatorContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ElectoralСalculator.sdf");
            optionsBuilder.UseSqlCe(@"Data Source=" + dbPath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Seller>()
            //    .HasIndex(b => b.WorkId)
            //    .IsUnique();
            //modelBuilder.Entity<Purchase>()
        }
    }
}
