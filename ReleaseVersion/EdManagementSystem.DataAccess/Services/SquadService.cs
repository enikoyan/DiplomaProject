using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class SquadService : ISquadService
    {
        private readonly EdSystemDbContext _context;

        public SquadService(EdSystemDbContext context)
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

        public async Task<List<int>> GetSquadsIdsByCourse(int courseId)
        {
            var squadIds = await _context.Squads
                .Where(s => s.IdCourse == courseId)
                .Select(s => s.SquadId)
                .ToListAsync();

            return squadIds;
        }

        public async Task<Squad> GetSquadByName(string squadName)
        {
            var squad = await _context.Squads.FirstOrDefaultAsync(u => u.OptionValue == squadName);
            if (squad == null)
            {
                throw new Exception("Группа не найдена!");
            }
            return squad;
        }

        public async Task<int> GetSquadIdByName(string squadName)
        {
            var squadId = await _context.Squads
                    .Where(u => u.OptionValue == squadName)
                    .Select(u => u.SquadId)
                    .FirstOrDefaultAsync();

            if (squadId == 0)
            {
                throw new Exception("Группа не найдена!");
            }
            return squadId;
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

        public async Task<List<int>> GetCoursesIdsByNames(List<int> squadIds)
        {
            List<int> result = new List<int>();

            foreach (var squadId in squadIds)
            {
                var foundItem = await GetCourseIdBySquadId(squadId);
                result.Add(foundItem);
            }

            return result;
        }

        public async Task<List<int>> GetSquadsIdsByNames(List<string> squadNames)
        {
            List<int> result = new List<int>();

            foreach (var squadName in squadNames)
            {
                var foundItem = await GetSquadIdByName(squadName);

                result.Add(foundItem);
            }

            return result;
        }

        private async Task<int> GetCourseIdBySquadId(int squadId)
        {
            return await _context.Squads
                    .Where(s => s.SquadId == squadId)
                    .Select(s => s.IdCourse)
                    .FirstOrDefaultAsync();
        }
    }
}
