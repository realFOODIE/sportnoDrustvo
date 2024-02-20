using Microsoft.EntityFrameworkCore; //uporablja entitiy framework core za delo z bazo podatkov
using Microsoft.EntityFrameworkCore.Design; //za update databbse ter migrationi
using Microsoft.Extensions.Configuration;
using static sportnoDrustvo.Classes.Models; //uvozi modele iz namespace-a sportnoDrustvo.Classes.Models za lažje dostopanje

namespace sportnoDrustvo.Classes
{
    //razred ApplicationDbContext razširja DbContext in služi kot osrednja točka za komunikacijo z bazo podatkov
    public class ApplicationDbContext : DbContext
    {
        //DbSet<T> predstavlja tabele v bazi podatkov in omogoča opravljanje CRUD operacij na njih
        public DbSet<Termin> Termini { get; set; }
      
        public DbSet<Obvestilo> Obvestila { get; set; }
    
        public DbSet<Rekvizit> Rekviziti { get; set; }
    
        public DbSet<Trener> Trenerji { get; set; }
       
        public DbSet<Clan> Clani { get; set; }
        
        public DbSet<Rezervacija> Rezervacije { get; set; }

        //Konstruktor ApplicationDbContext sprejme konfiguracijske opcije in jih posreduje nadrejenemu razredu DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //OnModelCreating se uporablja za konfiguracijo modelov in njihovih odnosov med seboj
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //pokliče implementacijo nadrejenega razreda
        }
    }
}
