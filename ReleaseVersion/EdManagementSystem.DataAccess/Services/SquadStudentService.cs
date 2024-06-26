﻿using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdManagementSystem.DataAccess.Services
{
    public class SquadStudentService : ISquadStudentService
    {
        private readonly EdSystemDbContext _context;

        public SquadStudentService(EdSystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSquadStudent(int studentId, int squadId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var squad = await _context.Squads.FindAsync(squadId);

            if (student == null || squad == null)
            {
                return false;
            }

            // Проверка, что студент уже не прикреплен к данному курсу
            var existingSquadStudent = await _context.SquadStudents
                .FirstOrDefaultAsync(ss => ss.IdStudent == studentId && ss.IdSquad == squadId);

            if (existingSquadStudent != null)
            {
                return false;
            }

            var squadStudent = new SquadStudent
            {
                IdStudent = studentId,
                IdSquad = squadId,
                AttachedDate = DateTime.Now
            };

            _context.SquadStudents.Add(squadStudent);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<int>> GetStudentsIdsBySquads(List<int> squadsIds)
        {
            var studentIds = await _context.SquadStudents
                   .Where(s => squadsIds.Contains(s.IdSquad))
                   .Select(s => s.IdStudent)
                   .ToListAsync();

            return studentIds;
        }

        public async Task<List<int>> GetStudentsIdsBySquad(int squadId)
        {
            var studentIds = await _context.SquadStudents
                   .Where(s => s.IdSquad == squadId)
                   .Select(s => s.IdStudent)
                   .ToListAsync();

            return studentIds;
        }
    }
}
