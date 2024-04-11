using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IMaterialService
    {
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid materialId);
        Task<Material> GetMaterialByTitle(string materialTitle);
        Task<List<Material>> GetMaterialsByType(string fileType);
        Task<List<Material>> GetMaterialsBySquad(string squadName);
        Task<List<Material>> GetMaterialsByCourse(string courseName);
        Task<bool> CreateMaterial(List<IFormFile> files, string groupBy, List<string> foreignKeys);
        Task<IActionResult> DownloadMaterial(Guid materialId);
        Task<bool> DeleteSquadMaterial(Guid materialId, string squadName);
        Task<bool> DeleteCourseMaterial(Guid materialId, string courseName);
    }
}