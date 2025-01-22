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
     public DbSet<VoertuigStatus> VoertuigStatus { get; set; }
     public DbSet<SchadeFormulier> SchadeFormulier { get; set; }
     public DbSet<Abonnement> Abonnementen { get; set; }
     public DbSet<AbonnementWagenparkLinked> AbonnementWagenparkLinked{ get; set; }
     public DbSet<NieuwWagenParkVerzoek> NieuwWagenParkVerzoek {get; set;}
     public DbSet<WagenParkUserList> WagenParkUserLists { get; set; }

     protected override void OnModelCreating(ModelBuilder builder){
        {
            builder.Entity<Voertuig>()
                .HasOne(v => v.voertuigStatus)
                .WithOne(vs => vs.Voertuig)
                .HasForeignKey<VoertuigStatus>(vs => vs.VoertuigId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
        }
        base.OnModelCreating(builder);
    }

  
    }
    
}