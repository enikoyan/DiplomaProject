using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EdManagementSystem.DataAccess.Services
{
    public class AuthService: IAuthService
    {
        private readonly User004Context _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(User004Context context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public User Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserEmail == email);

            if (user == null)
            {
                return null!;
            }

            else
            {
                if (_passwordHasher.Verify(password, user.UserPassword))
                {
                    return user;
                }

                else return null!;
            }
        }
    }
}
