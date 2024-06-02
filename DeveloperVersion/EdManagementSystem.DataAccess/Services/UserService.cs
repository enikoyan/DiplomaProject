using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly EdSystemDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(EdSystemDbContext context, IPasswordHasher passwordHashed)
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

        public async Task<bool> ChangePassword(User user, string newPassword)
        {
            try
            {
                var changedPassword = _passwordHasher.Generate(newPassword);
                user.UserPassword = changedPassword;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
