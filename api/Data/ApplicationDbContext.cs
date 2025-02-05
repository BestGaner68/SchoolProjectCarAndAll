using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    /// <summary>
    /// deze klasse omtreft de hele database en structuur hiervan, de dbset zijn de tabellen en de onmodelbuilding geeft een aantal extra requirements voor het genereren van de migration
    /// dit doen we om het duidelijker te maken voor ef core wat er gemapt moet worden en welke verbindingen er zijn
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<AppUser> 
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        
        }
        public DbSet<Voertuig> Voertuig{ get; set; }
        public DbSet<VerhuurVerzoek> VerhuurVerzoek { get; set; }
        public DbSet<WagenPark> Wagenpark { get; set; }
        public DbSet<Reservering> Reservering { get; set; }
        public DbSet<VoertuigData> VoertuigData { get; set; }
        public DbSet<SchadeFormulier> SchadeFormulier { get; set; }
        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<NieuwWagenParkVerzoek> NieuwWagenParkVerzoek {get; set;}
        public DbSet<WagenParkUserList> WagenParkUserLists { get; set; }
        public DbSet<WagenparkAbonnementen> WagenparkAbonnementen { get; set; }
        public DbSet<UserAbonnement> UserAbonnementen { get; set; }
        public DbSet<Accessoires> Accessoires{ get; set; }
        public DbSet<Verzekering> Verzekeringen { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            {
                builder.Entity<Abonnement>()
                    .Property(a => a.Prijs)
                    .HasPrecision(18, 2);

                builder.Entity<VoertuigData>()
                    .Property(a => a.KilometerPrijs)
                    .HasPrecision(18, 2);

                builder.Entity<UserAbonnement>()
                    .HasOne(ua => ua.AppUser)
                    .WithMany(au => au.UserAbonnementen)
                    .HasForeignKey(ua => ua.AppUserId);

                builder.Entity<UserAbonnement>()
                    .HasOne(ua => ua.Abonnement)
                    .WithMany(a => a.UserAbonnementen)
                    .HasForeignKey(ua => ua.AbonnementId);

                builder.Entity<VerhuurVerzoek>()
                    .HasOne(v => v.Verzekering)
                    .WithMany()
                    .HasForeignKey(v => v.VerzekeringId);

                builder.Entity<Reservering>()
                    .HasOne(r => r.Verzekering)
                    .WithMany()
                    .HasForeignKey(r => r.VerzekeringId);

                builder.Entity<Accessoires>()
                    .Property(a => a.Prijs)
                    .HasPrecision(18, 2);

                builder.Entity<VerhuurVerzoek>()
                    .HasMany(a => a.Accessoires)
                    .WithMany();

                builder.Entity<Reservering>()
                    .HasMany(a => a.Accessoires)
                    .WithMany();  

                builder.Entity<Verzekering>()
                    .Property(v => v.VerzekeringPrijs)
                    .HasPrecision(18, 2);
            }
        base.OnModelCreating(builder);
            }
    }
}
    
