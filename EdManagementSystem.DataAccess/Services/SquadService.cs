using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class SquadService : ISquadService
    {
        private readonly User004Context _context;

        public SquadService(User004Context context)
        {
            _context = context;
        }

        public async Task<Squad> GetSquadById(int squadId)
        {
            var squad = await _context.Squads.FirstOrDefaultAsync(u => u.SquadId == squadId);
            if (squad == null)
            {
                throw new Exception("Группа не найдена!");
            }
            return squad;
        }

        public async Task<List<Squad>> GetSquadsByCourse(int courseId)
        {
            var squads = await _context.Squads.Where(u => u.IdCourse == courseId).ToListAsync();
            if (squads == null || squads.Count == 0)
            {
                throw new Exception("Группы не найдены!");
            }
            return squads;
        }

        public async Task<Squad> GetSquadByName(string squadName)
        {
            var squad = await _context.Squads.FirstOrDefaultAsync(u => u.SquadName == squadName);
            if (squad == null)
            {
                throw new Exception("Группа не найдена!");
            }
            return squad;
        }

        public async Task DeleteSquad(int squadId)
        {
            var squad = await _context.Squads.FindAsync(squadId);
            if (squad != null)
            {
                _context.Squads.Remove(squad);
                await _context.SaveChangesAsync();
            }
            else throw new Exception("Группа не найдена!");
        }

        public async Task DeleteSquad(string squadName)
        {
            var squad = await _context.Squads.FindAsync(squadName);
            if (squad != null)
            {
                _context.Squads.Remove(squad);
                await _context.SaveChangesAsync();
            }
            else throw new Exception("Группа не найдена!");
        }

        public async Task<List<Squad>> GetAllSquads()
        {
            return await _context.Squads.ToListAsync();
        }

        public async Task<Squad> CreateSquad(Squad squad)
        {
            var existingCourse = await _context.Courses.FindAsync(squad.IdCourse);

            if (existingCourse == null)
            {
                throw new Exception("Курс не найден!");
            }

            var existingSquad = await _context.Squads.FirstOrDefaultAsync(s => s.SquadName == squad.SquadName);

            if (existingSquad != null)
            {
                throw new Exception("Группа с таким именем уже существует!");
            }

            _context.Entry(squad).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return squad;
        }
    }
}
