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

     protected override void OnModelCreating(ModelBuilder builder){
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

            base.OnModelCreating(builder);
        }
    }

  
    }
    
}