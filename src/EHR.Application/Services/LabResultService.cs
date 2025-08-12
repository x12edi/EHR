// EHR.Application/Services/LabResultService.cs
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
    public interface ILabResultService
    {
        Task<PagedResponse<LabResultDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<LabResultDto> GetByIdAsync(Guid id);
        Task<LabResultDto> CreateAsync(CreateLabResultDto dto);
        Task<LabResultDto> UpdateAsync(UpdateLabResultDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class LabResultService : ILabResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabResultService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<LabResultDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<LabResult>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(l =>
                    l.TestName.Contains(search) ||
                    l.TestCode.Contains(search) ||
                    l.Value.Contains(search) ||
                    l.Flag.Contains(search) ||
                    l.ReportedBy.Contains(search)
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

            var dtoItems = _mapper.Map<List<LabResultDto>>(items);

            return new PagedResponse<LabResultDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<LabResultDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<LabResult>().GetByIdAsync(id);
            return _mapper.Map<LabResultDto>(entity);
        }

        public async Task<LabResultDto> CreateAsync(CreateLabResultDto dto)
        {
            var entity = _mapper.Map<LabResult>(dto);
            await _unitOfWork.Repository<LabResult>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LabResultDto>(entity);
        }

        public async Task<LabResultDto> UpdateAsync(UpdateLabResultDto dto)
        {
            var entity = await _unitOfWork.Repository<LabResult>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("LabResult not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<LabResult>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LabResultDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<LabResult>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<LabResult>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
