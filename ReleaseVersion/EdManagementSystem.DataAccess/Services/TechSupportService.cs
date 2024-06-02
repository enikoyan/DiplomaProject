using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class TechSupportService : ITechSupportService
    {
        private readonly EdSystemDbContext _context;

        public TechSupportService(EdSystemDbContext context)
        {
            _context = context;
        }

        public async Task<List<TechSupport>> GetAllRequests()
        {
            return await _context.TechSupports.ToListAsync();
        }

        public async Task<TechSupport> GetRequestById(int requestId)
        {
            var request = await _context.TechSupports.FirstOrDefaultAsync(u => u.Id == requestId);

            if (request == null)
            {
                throw new Exception("Обращение не найдено!");
            }
            return request;
        }

        public async Task<List<TechSupport>> GetUserRequests(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == userEmail);

            if (user == null)
            {
                throw new Exception("Такого пользователя нет!");
            }

            var requests = _context.TechSupports.Where(s => s.IdUser == user.UserId).ToList();

            return requests;
        }

        public async Task<List<TechSupport>> GetRequestsByStatus(string requestStatus)
        {
            return await _context.TechSupports.Where(s => s.Status == requestStatus).ToListAsync();
        }

        public async Task<bool> CreateRequest(string userEmail, string requestDescription)
        {
            if (String.IsNullOrEmpty(userEmail) || String.IsNullOrEmpty(requestDescription))
            {
                throw new Exception("Заполните все поля!");
            }
            else
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

                if (user == null)
                {
                    throw new Exception("Пользователь не найден!");
                }

                var newRequest = new TechSupport
                {
                    IdUser = user.UserId,
                    Description = requestDescription,
                    DateCreation = DateTime.UtcNow,
                    Status = "в обработке"
                };

                _context.TechSupports.Add(newRequest);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> ChangeRequestStatus(int requestId, string newStatus)
        {
            var request = await _context.TechSupports.FindAsync(requestId);

            if (request == null)
            {
                throw new Exception("Обращение не найдено!");
            }

            request.Status = newStatus;
            _context.Update(request);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
