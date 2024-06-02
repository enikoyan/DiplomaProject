using EdManagementSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IAuthService
    {
        User Authenticate(string email, string password);
    }
}
