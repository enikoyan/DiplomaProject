using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using EdManagementSystem.DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SquadsController : ControllerBase
    {
        private readonly ISquadService _squadService;

        public SquadsController(ISquadService squadService)
        {
            _squadService = squadService;
        }

        [HttpGet("{squadId}")]
        public async Task<ActionResult<Squad>> GetSquadById(int squadId)
        {
            try
            {
                var squad = await _squadService.GetSquadById(squadId);
                return Ok(squad);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<List<Squad>>> GetSquadsByCourse(int courseId)
        {
            try
            {
                var squads = await _squadService.GetSquadsByCourse(courseId);
                return Ok(squads);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{squadName}")]
        public async Task<ActionResult<Squad>> GetSquadByName(string squadName)
        {
            try
            {
                var squad = await _squadService.GetSquadByName(squadName);
                return Ok(squad);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Squad>> CreateSquad(Squad squad)
        {
            try
            {
                var createdSquad = await _squadService.CreateSquad(squad);
                return Ok(createdSquad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{squadId}")]
        public async Task<ActionResult> DeleteSquadById(int squadId)
        {
            try
            {
                await _squadService.DeleteSquad(squadId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{squadName}")]
        public async Task<ActionResult> DeleteSquadByName(string squadName)
        {
            try
            {
                await _squadService.DeleteSquad(squadName);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Squad>>> GetAllSquads()
        {
            try
            {
                var squads = await _squadService.GetAllSquads();
                return Ok(squads);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{courseId}")]
        public async Task<IActionResult> GetSquadsIdsByCourse(int courseId)
        {
            var squadIds = await _squadService.GetSquadsIdsByCourse(courseId);

            if (squadIds == null || squadIds.Count == 0)
            {
                return NotFound();
            }

            return Ok(squadIds);
        }
    }
}
