using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;

namespace api.Repositories
{
    public class AbonnementServiceRepo :IAbonnementService
    {
        private readonly ApplicationDbContext _context;
        public AbonnementServiceRepo(ApplicationDbContext context)
        {
            _context=context;
        }
    }
}