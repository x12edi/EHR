// EHR.Application/Services/ScheduleSlotService.cs
using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Repositories.Interfaces;
using EHR.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHR.Application.Services
{
    public interface IScheduleSlotService
    {
        Task<PagedResponse<ScheduleSlotDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ScheduleSlotDto> GetByIdAsync(Guid id);
        Task<ScheduleSlotDto> CreateAsync(CreateScheduleSlotDto dto);
        Task<ScheduleSlotDto> UpdateAsync(UpdateScheduleSlotDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ScheduleSlotService : IScheduleSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ScheduleSlotDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<ScheduleSlot>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                // No direct textual field for search except recurrence, so optional search here
                query = query.Where(s => s.RecurrenceJson.Contains(search));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isAscending
                    ? query.OrderByDynamic(sortBy, true)
                    : query.OrderByDynamic(sortBy, false);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtoItems = _mapper.Map<List<ScheduleSlotDto>>(items);

            return new PagedResponse<ScheduleSlotDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ScheduleSlotDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ScheduleSlot>().GetByIdAsync(id);
            return _mapper.Map<ScheduleSlotDto>(entity);
        }

        public async Task<ScheduleSlotDto> CreateAsync(CreateScheduleSlotDto dto)
        {
            var entity = _mapper.Map<ScheduleSlot>(dto);
            await _unitOfWork.Repository<ScheduleSlot>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ScheduleSlotDto>(entity);
        }

        public async Task<ScheduleSlotDto> UpdateAsync(UpdateScheduleSlotDto dto)
        {
            var entity = await _unitOfWork.Repository<ScheduleSlot>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("ScheduleSlot not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<ScheduleSlot>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ScheduleSlotDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ScheduleSlot>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<ScheduleSlot>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
