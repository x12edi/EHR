// EHR.Application/Services/PatientService.cs
using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Extensions;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EHR.Application.Services
{
    public interface IPatientService
    {
        Task<PagedResponse<PatientDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<PatientDto> GetByIdAsync(Guid id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task<PatientDto> UpdateAsync(UpdatePatientDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor http)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _http = http;
        }

        private string? CurrentUserId =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        private string getFullName(string? first, string? middle, string? last) =>
            string.Join(" ", new[] { first, middle, last }.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!.Trim()));


        public async Task<PagedResponse<PatientDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Patient>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.MRN.Contains(search) ||
                    p.FirstName.Contains(search) ||
                    p.LastName.Contains(search) ||
                    p.FullNameNormalized.Contains(search) ||
                    p.PrimaryPhone.Contains(search) ||
                    p.Email.Contains(search)
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

            var dtoItems = _mapper.Map<List<PatientDto>>(items);

            return new PagedResponse<PatientDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
            return _mapper.Map<PatientDto>(entity);
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            var entity = _mapper.Map<Patient>(dto);
            entity.FullNameNormalized = getFullName(dto.FirstName, dto.MiddleName, dto.LastName);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = CurrentUserId;

            await _unitOfWork.Repository<Patient>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PatientDto>(entity);
        }

        public async Task<PatientDto> UpdateAsync(UpdatePatientDto dto)
        {
            var entity = await _unitOfWork.Repository<Patient>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Patient not found.");

            _mapper.Map(dto, entity);
            entity.FullNameNormalized = getFullName(dto.FirstName, dto.MiddleName, dto.LastName);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = CurrentUserId;
            _unitOfWork.Repository<Patient>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PatientDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Patient>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
