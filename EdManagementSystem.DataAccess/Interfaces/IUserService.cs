using EdManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByEmail(string userEmail);
        Task CreateUser(User user);
        Task DeleteUser(int userId);
    }
}
