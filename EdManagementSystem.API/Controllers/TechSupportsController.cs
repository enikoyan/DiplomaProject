using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TechSupportsController : ControllerBase
    {
        private readonly ITechSupportService _techSupportService;

        public TechSupportsController(ITechSupportService techSupportService)
        {
            _techSupportService = techSupportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TechSupport>>> GetAllRequests()
        {
            try
            {
                var requests = await _techSupportService.GetAllRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{requestId}")]
        public async Task<ActionResult<TechSupport>> GetRequestById(int requestId)
        {
            try
            {
                var request = await _techSupportService.GetRequestById(requestId);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{requestStatus}")]
        public async Task<ActionResult<List<TechSupport>>> GetRequestsByStatus(string requestStatus)
        {
            if (!Statuses.Contains(requestStatus))
            {
                return BadRequest("Такого статуса нет, смените на валидный (в обработке, обработано)");
            }

            try
            {
                var requests = await _techSupportService.GetRequestsByStatus(requestStatus);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userEmail}")]
        public async Task<ActionResult<List<TechSupport>>> GetUserRequests(string userEmail)
        {
            try
            {
                var requests = await _techSupportService.GetUserRequests(userEmail);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateRequest([FromBody] TechSupportRequestModel model)
        {
            try
            {
                var result = await _techSupportService.CreateRequest(model.UserEmail, model.RequestDescription);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> ChangeRequestStatus(int idRequest, string newStatus)
        {
            if (!Statuses.Contains(newStatus))
            {
                return BadRequest("Такого статуса нет, смените на валидный (в обработке, обработано)");
            }
            else
            {
                try
                {
                    var result = await _techSupportService.ChangeRequestStatus(idRequest, newStatus);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        public List<string> Statuses = new List<string>()
        {
            "в обработке",
            "обработано",
        };
    }

    public class TechSupportRequestModel
    {
        public required string UserEmail { get; set; }
        private string _requestDescription = null!;
        public string RequestDescription
        {
            get { return _requestDescription; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Запрос не может быть пустым!");
                }
                if (value.Trim().Length < 250)
                {
                    throw new ArgumentException("Запрос не может быть меньше 250 символов!");
                }

                _requestDescription = value;
            }
        }
    }
}
