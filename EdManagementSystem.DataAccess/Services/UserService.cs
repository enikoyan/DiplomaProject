using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly User004Context _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(User004Context context, IPasswordHasher passwordHashed)
        {
            _context = context;
            _passwordHasher = passwordHashed;
        }

        public async Task<User> GetUserByEmail(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
            if (user == null)
            {
                throw new Exception("Пользователь не найден!");
            }
            return user;
        }

        public async Task CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            string hashedPassword = _passwordHasher.Generate(user.UserPassword);

            user.UserPassword = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
