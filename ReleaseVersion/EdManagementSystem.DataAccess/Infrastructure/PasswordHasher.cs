using BCrypt.Net;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
