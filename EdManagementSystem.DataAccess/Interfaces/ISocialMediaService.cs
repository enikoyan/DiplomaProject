using EdManagementSystem.DataAccess.Services;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ISocialMediaService
    {
        Task AddSocialMedium(int idTeacher, string socialMediaName, string socialMediaUrl);
        Task<List<SocialMediaItem>> GetSocialMediaById(string teacherEmail);
    }
}