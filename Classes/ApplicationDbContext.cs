using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using static sportnoDrustvo.Classes.Models;

namespace sportnoDrustvo.Classes
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Termin> Termini { get; set; }
        public DbSet<Obvestilo> Obvestila { get; set; }
        public DbSet<Rekvizit> Rekviziti { get; set; }
        public DbSet<Trener> Trenerji { get; set; }
        public DbSet<Clan> Clani { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Konfiguracija in odnosi med modeli
        }

    

    }
}
