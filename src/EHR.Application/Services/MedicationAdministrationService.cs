// EHR.Application/Services/MedicationAdministrationService.cs
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
    public interface IMedicationAdministrationService
    {
        Task<PagedResponse<MedicationAdministrationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<MedicationAdministrationDto> GetByIdAsync(Guid id);
        Task<MedicationAdministrationDto> CreateAsync(CreateMedicationAdministrationDto dto);
        Task<MedicationAdministrationDto> UpdateAsync(UpdateMedicationAdministrationDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class MedicationAdministrationService : IMedicationAdministrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicationAdministrationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<MedicationAdministrationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<MedicationAdministration>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.DoseGiven.Contains(search) ||
                    m.Route.Contains(search) ||
                    m.Notes.Contains(search)
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

            var dtoItems = _mapper.Map<List<MedicationAdministrationDto>>(items);

            return new PagedResponse<MedicationAdministrationDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<MedicationAdministrationDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<MedicationAdministration>().GetByIdAsync(id);
            return _mapper.Map<MedicationAdministrationDto>(entity);
        }

        public async Task<MedicationAdministrationDto> CreateAsync(CreateMedicationAdministrationDto dto)
        {
            var entity = _mapper.Map<MedicationAdministration>(dto);
            await _unitOfWork.Repository<MedicationAdministration>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MedicationAdministrationDto>(entity);
        }

        public async Task<MedicationAdministrationDto> UpdateAsync(UpdateMedicationAdministrationDto dto)
        {
            var entity = await _unitOfWork.Repository<MedicationAdministration>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("MedicationAdministration not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<MedicationAdministration>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<MedicationAdministrationDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<MedicationAdministration>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<MedicationAdministration>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
