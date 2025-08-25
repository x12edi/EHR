// API/Controllers/ProvidersController.cs
using EHR.Application.DTOs;
using EHR.Application.Parameters;
using EHR.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        private string CurrentUser => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameter pagination, [FromQuery] string search = null, [FromQuery] string sortBy = null, [FromQuery] bool isAscending = true)
        {
            var result = await _providerService.GetAllAsync(pagination, search, sortBy, isAscending);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _providerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProviderDto dto)
        {
            var result = await _providerService.CreateAsync(dto, CurrentUser);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProviderDto dto)
        {
            dto.Id = id;
            var result = await _providerService.UpdateAsync(dto, CurrentUser);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _providerService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
