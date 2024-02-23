using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class AuthService(User004Context context) : IAuthService
    {
        private readonly User004Context _context = context;

        public User Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserEmail == email && String.Equals(x.UserPassword, password));

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
