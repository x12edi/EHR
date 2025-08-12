using EHR.Application.DTOs;
using EHR.Application.Parameters;
using EHR.Application.Services;
using EHR.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace EHR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingRecordsController : ControllerBase
    {
        private readonly IBillingRecordService _billingRecordService;

        public BillingRecordsController(IBillingRecordService billingRecordService)
        {
            _billingRecordService = billingRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<BillingRecordDto>>> GetAll(
            [FromQuery] PaginationParameter pagination,
            [FromQuery] string search = null,
            [FromQuery] string sortBy = null,
            [FromQuery] bool isAscending = true)
        {
            return Ok(await _billingRecordService.GetAllAsync(pagination, search, sortBy, isAscending));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BillingRecordDto>> GetById(Guid id)
        {
            return Ok(await _billingRecordService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<BillingRecordDto>> Create(CreateBillingRecordDto dto)
        {
            var created = await _billingRecordService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBillingRecordDto dto)
        {
            await _billingRecordService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _billingRecordService.DeleteAsync(id);
            return NoContent();
        }
    }
}
