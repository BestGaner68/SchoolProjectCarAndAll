using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class DoubleDataCheckerRepo : IDoubleDataCheckerRepo
    {
        private readonly UserManager<AppUser> _userManager;
        public DoubleDataCheckerRepo(UserManager<AppUser> userManager, ApplicationDbContext context){
            _userManager = userManager;
        }
        public async Task<bool> UsernameTaken (string username){
            var existingUsername = await _userManager.FindByNameAsync(username);
            return existingUsername != null;
        }

        public async Task<bool> PhoneNumberTaken (string phoneNumber){
            var existingPhone = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            return existingPhone != null;
        }

        public async Task<bool> EmailTaken (string Email){
            var existingEmail = await _userManager.FindByEmailAsync(Email);
            return existingEmail != null;
        }
    }
}
