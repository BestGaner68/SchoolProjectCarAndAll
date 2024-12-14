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
     public DbSet<Auto> Auto{ get; set; }
     public DbSet<VerhuurVerzoek> verhuurVerzoek { get; set; }
     public DbSet<WagenPark> wagenPark { get; set; }
     public DbSet<WagenparkLinkedUser> wagenparkUserLinked { get; set; }
     public DbSet<WagenParkVerzoek> wagenparkVerzoeken {get; set;}

     protected override void OnModelCreating(ModelBuilder builder){
        builder.Entity<WagenparkLinkedUser>()
            .HasKey(x => new { x.WagenparkId, x.AppUserId });

        base.OnModelCreating(builder);


        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "frontendWorker", NormalizedName = "FRONTENDWORKER" },
            new IdentityRole { Name = "backendWorker", NormalizedName = "BACKENDWORKER" },
            new IdentityRole { Name = "particuliereKlant", NormalizedName = "PARTICULIEREKLANT" },
            new IdentityRole { Name = "bedrijflant", NormalizedName = "BEDRIJFKLANT" },
            new IdentityRole { Name = "wagenparkBeheerder", NormalizedName = "WAGENPARKBEHEERDER" }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }

  
    }
    
}