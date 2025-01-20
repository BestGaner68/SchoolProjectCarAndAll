using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Service
{
    public static class RoleInitializer
    {
        public static async Task InitializeRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "backendWorker", "wagenparkBeheerder", "particuliereKlant", "frontendWorker", "bedrijfsKlant"};
        
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}