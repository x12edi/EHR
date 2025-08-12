// EHR.Application/Services/MedicationRequestService.cs
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
    public interface IMedicationRequestService
    {
        Task<PagedResponse<MedicationRequestDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<MedicationRequestDto> GetByIdAsync(Guid id);
        Task<MedicationRequestDto> CreateAsync(CreateMedicationRequestDto dto);
        Task<MedicationRequestDto> UpdateAsync(UpdateMedicationRequestDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class MedicationRequestService : IMedicationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicationRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<MedicationRequestDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<MedicationRequest>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.MedicationText.Contains(search) ||
                    m.MedicationCode.Contains(search) ||
                    m.Status.Contains(search) ||
                    m.Reason.Contains(search)
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

            var dtoItems = _mapper.Map<List<MedicationRequestDto>>(items);

            return new PagedResponse<MedicationRequestDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<MedicationRequestDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<MedicationRequest>().GetByIdAsync(id);
            return _mapper.Map<MedicationRequestDto>(entity);
        }

        public async Task<MedicationRequestDto> CreateAsync(CreateMedicationRequestDto dto)
        {
            var entity = _mapper.Map<MedicationRequest>(dto);
            await _unitOfWork.Repository<MedicationRequest>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MedicationRequestDto>(entity);
        }

        public async Task<MedicationRequestDto> UpdateAsync(UpdateMedicationRequestDto dto)
        {
            var entity = await _unitOfWork.Repository<MedicationRequest>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("MedicationRequest not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<MedicationRequest>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MedicationRequestDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<MedicationRequest>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<MedicationRequest>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
