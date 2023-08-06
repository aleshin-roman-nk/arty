using Arty.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
    public class AppDataDb: DbContext, IAppDbFactory
    {
        string _path;

        public AppDataDb(string path) : base()
        {
            _path = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_path}");
			
			//optionsBuilder.LogTo(Console.WriteLine);
			//optionsBuilder.EnableSensitiveDataLogging(true);
		}

        public AppDataDb Create()
        {
            return new AppDataDb(_path);
        }

        public DbSet<PersonalTerritory> PersonalTerritories { get; set; }
        public DbSet<AreaLine> Lines { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<AppProperty> AppProperties { get; set; }
        public DbSet<StreetLine> StreetLines { get; set; }
        public DbSet<Building> Buildings { get; set; }
    }
}
