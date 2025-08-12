// EHR.Application/Services/EncounterParticipantService.cs
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
    public interface IEncounterParticipantService
    {
        Task<PagedResponse<EncounterParticipantDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<EncounterParticipantDto> GetByIdAsync(Guid id);
        Task<EncounterParticipantDto> CreateAsync(CreateEncounterParticipantDto dto);
        Task<EncounterParticipantDto> UpdateAsync(UpdateEncounterParticipantDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class EncounterParticipantService : IEncounterParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EncounterParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<EncounterParticipantDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<EncounterParticipant>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Role.Contains(search)
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

            var dtoItems = _mapper.Map<List<EncounterParticipantDto>>(items);

            return new PagedResponse<EncounterParticipantDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<EncounterParticipantDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<EncounterParticipant>().GetByIdAsync(id);
            return _mapper.Map<EncounterParticipantDto>(entity);
        }

        public async Task<EncounterParticipantDto> CreateAsync(CreateEncounterParticipantDto dto)
        {
            var entity = _mapper.Map<EncounterParticipant>(dto);
            await _unitOfWork.Repository<EncounterParticipant>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EncounterParticipantDto>(entity);
        }

        public async Task<EncounterParticipantDto> UpdateAsync(UpdateEncounterParticipantDto dto)
        {
            var entity = await _unitOfWork.Repository<EncounterParticipant>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("EncounterParticipant not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<EncounterParticipant>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EncounterParticipantDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<EncounterParticipant>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<EncounterParticipant>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
