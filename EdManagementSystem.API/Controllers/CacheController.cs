using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost]
        public IActionResult InvalidateCache(string key)
        {
            bool result = _cacheService.InvalidateCache(key);

            if (result)
            {
                return Ok("Cache invalidated successfully");
            }
            else
            {
                return BadRequest("Failed to invalidate cache");
            }
        }
    }
}
