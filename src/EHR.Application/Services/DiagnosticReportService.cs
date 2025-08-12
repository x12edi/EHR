// EHR.Application/Services/DiagnosticReportService.cs
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
    public interface IDiagnosticReportService
    {
        Task<PagedResponse<DiagnosticReportDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<DiagnosticReportDto> GetByIdAsync(Guid id);
        Task<DiagnosticReportDto> CreateAsync(CreateDiagnosticReportDto dto);
        Task<DiagnosticReportDto> UpdateAsync(UpdateDiagnosticReportDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class DiagnosticReportService : IDiagnosticReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiagnosticReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<DiagnosticReportDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<DiagnosticReport>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    r.ReportType.Contains(search) ||
                    r.Summary.Contains(search)
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

            var dtoItems = _mapper.Map<List<DiagnosticReportDto>>(items);

            return new PagedResponse<DiagnosticReportDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<DiagnosticReportDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<DiagnosticReport>().GetByIdAsync(id);
            return _mapper.Map<DiagnosticReportDto>(entity);
        }

        public async Task<DiagnosticReportDto> CreateAsync(CreateDiagnosticReportDto dto)
        {
            var entity = _mapper.Map<DiagnosticReport>(dto);
            await _unitOfWork.Repository<DiagnosticReport>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DiagnosticReportDto>(entity);
        }

        public async Task<DiagnosticReportDto> UpdateAsync(UpdateDiagnosticReportDto dto)
        {
            var entity = await _unitOfWork.Repository<DiagnosticReport>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("DiagnosticReport not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<DiagnosticReport>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DiagnosticReportDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<DiagnosticReport>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<DiagnosticReport>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
