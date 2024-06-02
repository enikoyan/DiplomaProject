using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdManagementSystem.DataAccess.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly EdSystemDbContext _context;

        public SocialMediaService(EdSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddSocialMedium(int idTeacher, string socialMediaName, string socialMediaUrl)
        {
            if (!Enum.IsDefined(typeof(SocialMediaEnum), socialMediaName))
            {
                throw new ArgumentException("Такой соц.сети нет, выберите другую!");
            }

            // Check if the social media link already exists for the user
            var existingSocialMedium = await _context.SocialMedia
                .FirstOrDefaultAsync(sm => sm.IdTeacher == idTeacher && sm.SocialMediaName == socialMediaName);

            if (existingSocialMedium != null)
            {
                throw new ArgumentException("У пользователя уже существует ссылка на данную соц.сеть!");
            }

            var socialMedium = new SocialMedium
            {
                IdTeacher = idTeacher,
                SocialMediaName = socialMediaName,
                SocialMediaUrl = socialMediaUrl
            };

            _context.Add(socialMedium);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SocialMediaItem>> GetSocialMediaById(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            var socialMedia = _context.SocialMedia.Where(s => s.IdTeacher == teacher.TeacherId).ToList();

            var socialMediaItems = new List<SocialMediaItem>();

            foreach (var item in socialMedia)
            {
                socialMediaItems.Add(new SocialMediaItem
                {
                    SocialMediaName = item.SocialMediaName,
                    SocialMediaUrl = item.SocialMediaUrl
                });
            }

            return socialMediaItems;
        }
    }

    // Class that represents socialMediaItem
    public class SocialMediaItem
    {
        public required string SocialMediaName { get; set; }
        public required string SocialMediaUrl { get; set; }
    }

    public enum SocialMediaEnum
    {
        Telegram,
        Vk,
        Discord,
        Facebook
    }
}
