using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IMaterialService
    {
        Task<List<MaterialWithFile>> GetAllMaterials();
        Task<MaterialWithFile> GetMaterialById(Guid materialId);
        Task<MaterialWithFile> GetMaterialByTitle(string materialTitle);
        Task<List<MaterialWithFile>> GetMaterialsByType(string fileType);
        Task<List<MaterialWithFile>> GetMaterialsBySquad(string squadName);
        Task<List<MaterialWithFile>> GetMaterialsByCourse(string courseName);
        Task<bool> CreateMaterial(List<IFormFile> files, string groupBy, List<string> foreignKeys);
        Task<IActionResult> DownloadMaterial(Guid materialId);
        Task<bool> DeleteSquadMaterial(Guid materialId, string squadName);
        Task<bool> DeleteCourseMaterial(Guid materialId, string courseName);
    }
}