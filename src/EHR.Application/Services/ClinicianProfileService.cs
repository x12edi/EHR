// EHR.Application/Services/ClinicianProfileService.cs
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
    public interface IClinicianProfileService
    {
        Task<PagedResponse<ClinicianProfileDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ClinicianProfileDto> GetByIdAsync(Guid id);
        Task<ClinicianProfileDto> CreateAsync(CreateClinicianProfileDto dto);
        Task<ClinicianProfileDto> UpdateAsync(UpdateClinicianProfileDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ClinicianProfileService : IClinicianProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClinicianProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ClinicianProfileDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<ClinicianProfile>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.LicenseNumber.Contains(search) ||
                    c.NPI.Contains(search) ||
                    c.SpecialtyCode.Contains(search)
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

            var dtoItems = _mapper.Map<List<ClinicianProfileDto>>(items);

            return new PagedResponse<ClinicianProfileDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ClinicianProfileDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ClinicianProfile>().GetByIdAsync(id);
            return _mapper.Map<ClinicianProfileDto>(entity);
        }

        public async Task<ClinicianProfileDto> CreateAsync(CreateClinicianProfileDto dto)
        {
            var entity = _mapper.Map<ClinicianProfile>(dto);
            await _unitOfWork.Repository<ClinicianProfile>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ClinicianProfileDto>(entity);
        }

        public async Task<ClinicianProfileDto> UpdateAsync(UpdateClinicianProfileDto dto)
        {
            var entity = await _unitOfWork.Repository<ClinicianProfile>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Clinician profile not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<ClinicianProfile>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ClinicianProfileDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ClinicianProfile>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<ClinicianProfile>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
