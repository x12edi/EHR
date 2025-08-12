// EHR.API/Controllers/OutboxEventsController.cs
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EHR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutboxEventsController : ControllerBase
    {
        private readonly IOutboxEventService _service;

        public OutboxEventsController(IOutboxEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var result = await _service.GetAllAsync(pagination, search, sortBy, isAscending);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(long id)
        //{
        //    var result = await _service.GetByIdAsync(id);
        //    if (result == null) return NotFound();
        //    return Ok(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateOutboxEventDto dto)
        //{
        //    var result = await _service.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(long id, [FromBody] UpdateOutboxEventDto dto)
        //{
        //    if (id != dto.Id) return BadRequest();
        //    var result = await _service.UpdateAsync(dto);
        //    return Ok(result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    var deleted = await _service.DeleteAsync(id);
        //    if (!deleted) return NotFound();
        //    return NoContent();
        //}
    }
}
