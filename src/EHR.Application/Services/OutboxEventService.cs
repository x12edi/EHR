// EHR.Application/Services/OutboxEventService.cs
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
    public interface IOutboxEventService
    {
        Task<PagedResponse<OutboxEventDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        //Task<OutboxEventDto> GetByIdAsync(long id);
        Task<OutboxEventDto> CreateAsync(CreateOutboxEventDto dto);
        //Task<OutboxEventDto> UpdateAsync(UpdateOutboxEventDto dto);
        //Task<bool> DeleteAsync(long id);
    }
    public class OutboxEventService : IOutboxEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OutboxEventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<OutboxEventDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<OutboxEvent>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.EventType.Contains(search) ||
                    e.Status.Contains(search)
                );
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

            var dtoItems = _mapper.Map<List<OutboxEventDto>>(items);

            return new PagedResponse<OutboxEventDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        //public async Task<OutboxEventDto> GetByIdAsync(long id)
        //{
        //    var entity = await _unitOfWork.Repository<OutboxEvent>().GetByIdAsync(id);
        //    return _mapper.Map<OutboxEventDto>(entity);
        //}

        public async Task<OutboxEventDto> CreateAsync(CreateOutboxEventDto dto)
        {
            var entity = _mapper.Map<OutboxEvent>(dto);
            await _unitOfWork.Repository<OutboxEvent>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OutboxEventDto>(entity);
        }

        //public async Task<OutboxEventDto> UpdateAsync(UpdateOutboxEventDto dto)
        //{
        //    var entity = await _unitOfWork.Repository<OutboxEvent>().GetByIdAsync(dto.Id);
        //    if (entity == null)
        //        throw new KeyNotFoundException("OutboxEvent not found.");

        //    _mapper.Map(dto, entity);
        //    _unitOfWork.Repository<OutboxEvent>().Update(entity);
        //    await _unitOfWork.CommitAsync();
        //    return _mapper.Map<OutboxEventDto>(entity);
        //}

        //public async Task<bool> DeleteAsync(long id)
        //{
        //    var entity = await _unitOfWork.Repository<OutboxEvent>().GetByIdAsync(id);
        //    if (entity == null)
        //        return false;

        //    _unitOfWork.Repository<OutboxEvent>().Remove(entity);
        //    await _unitOfWork.CommitAsync();
        //    return true;
        //}
    }
}
