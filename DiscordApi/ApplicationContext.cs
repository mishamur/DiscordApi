using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiscordApi.LeagueApi.Models;

namespace DiscordApi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<PlayerGameStat> PlayerGameStats =>  Set<PlayerGameStat>();
        public ApplicationContext()
        {
           // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerGameStat>().HasKey(pgs => new {pgs.MatchId, pgs.UserPuuid});
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=consoleuserdb;Username=postgres;Password={p@ssw0rd}");
        }
       
    }
}
