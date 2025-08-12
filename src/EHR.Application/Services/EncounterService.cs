// EHR.Application/Services/EncounterService.cs
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
    public interface IEncounterService
    {
        Task<PagedResponse<EncounterDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<EncounterDto> GetByIdAsync(Guid id);
        Task<EncounterDto> CreateAsync(CreateEncounterDto dto);
        Task<EncounterDto> UpdateAsync(UpdateEncounterDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class EncounterService : IEncounterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EncounterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<EncounterDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Encounter>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.EncounterNumber.Contains(search) ||
                    e.EncounterType.Contains(search) ||
                    e.Status.Contains(search) ||
                    e.ChiefComplaint.Contains(search) ||
                    e.VisitReasonCode.Contains(search)
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

            var dtoItems = _mapper.Map<List<EncounterDto>>(items);

            return new PagedResponse<EncounterDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<EncounterDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Encounter>().GetByIdAsync(id);
            return _mapper.Map<EncounterDto>(entity);
        }

        public async Task<EncounterDto> CreateAsync(CreateEncounterDto dto)
        {
            var entity = _mapper.Map<Encounter>(dto);
            await _unitOfWork.Repository<Encounter>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EncounterDto>(entity);
        }

        public async Task<EncounterDto> UpdateAsync(UpdateEncounterDto dto)
        {
            var entity = await _unitOfWork.Repository<Encounter>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Encounter not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Encounter>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EncounterDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Encounter>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Encounter>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
