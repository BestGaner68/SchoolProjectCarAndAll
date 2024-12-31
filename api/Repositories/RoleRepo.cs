using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class RoleRepo : IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleRepo (ApplicationDbContext context){
            _context = context;
        }

        public async Task<string> getUserRole (string userID){
            
        var roleId = await _context.UserRoles
            .Where(ur => ur.UserId == userID) 
            .Select(ur => ur.RoleId)           
            .FirstOrDefaultAsync();

        if (roleId == null) return null;

        var roleName = await _context.Roles
            .Where(r => r.Id == roleId) 
            .Select(r => r.Name)        
            .FirstOrDefaultAsync();

        return roleName; 
        }
    }
}

   