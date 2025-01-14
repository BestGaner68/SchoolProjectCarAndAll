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
     public DbSet<WagenparkLinkedUser> WagenparkUserLinked { get; set; }
     public DbSet<WagenParkVerzoek> WagenparkVerzoeken {get; set;}
     public DbSet<Reservering> Reservering { get; set; }
     public DbSet<VoertuigStatus> VoertuigStatus { get; set; }
     public DbSet<SchadeFormulier> SchadeFormulier { get; set; }
     public DbSet<Abonnement> Abonnementen { get; set; }
     public DbSet<AbonnementUserLinked> AbonnementUserLinked{ get; set; }

     protected override void OnModelCreating(ModelBuilder builder){
        builder.Entity<WagenparkLinkedUser>()
            .HasKey(x => new { x.WagenparkId, x.AppUserId });

        base.OnModelCreating(builder);
    }

  
    }
    
}