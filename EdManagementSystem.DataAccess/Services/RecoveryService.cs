using EdManagementSystem.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdManagementSystem.DataAccess.Services
{
    public class RecoveryService
    {
        private readonly EdSystemDbContext _context;

        public RecoveryService(EdSystemDbContext context)
        {
            _context = context;
        }
    }
}
