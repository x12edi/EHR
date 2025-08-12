// Controllers/AuditLogsController.cs
using EHR.Application.DTOs;
using EHR.Application.Parameters;
using EHR.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EHR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly AuditLogService _service;

        public AuditLogsController(AuditLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameter pagination, [FromQuery] string search, [FromQuery] string sortBy, [FromQuery] bool isAscending = true)
        {
            var result = await _service.GetAllAsync(pagination, search, sortBy, isAscending);
            return Ok(result);
        }
    }
}
