// EHR.Application/Services/PatientIdentifierService.cs
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
    public interface IPatientIdentifierService
    {
        Task<PagedResponse<PatientIdentifierDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<PatientIdentifierDto> GetByIdAsync(Guid id);
        Task<PatientIdentifierDto> CreateAsync(CreatePatientIdentifierDto dto);
        Task<PatientIdentifierDto> UpdateAsync(UpdatePatientIdentifierDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class PatientIdentifierService : IPatientIdentifierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientIdentifierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PatientIdentifierDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<PatientIdentifier>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(pi =>
                    pi.IdentifierType.Contains(search) ||
                    pi.Value.Contains(search) ||
                    pi.Issuer.Contains(search)
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

            var dtoItems = _mapper.Map<List<PatientIdentifierDto>>(items);

            return new PagedResponse<PatientIdentifierDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<PatientIdentifierDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<PatientIdentifier>().GetByIdAsync(id);
            return _mapper.Map<PatientIdentifierDto>(entity);
        }

        public async Task<PatientIdentifierDto> CreateAsync(CreatePatientIdentifierDto dto)
        {
            var entity = _mapper.Map<PatientIdentifier>(dto);
            await _unitOfWork.Repository<PatientIdentifier>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PatientIdentifierDto>(entity);
        }

        public async Task<PatientIdentifierDto> UpdateAsync(UpdatePatientIdentifierDto dto)
        {
            var entity = await _unitOfWork.Repository<PatientIdentifier>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("PatientIdentifier not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<PatientIdentifier>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PatientIdentifierDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<PatientIdentifier>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<PatientIdentifier>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
