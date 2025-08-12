// EHR.Application/Services/DepartmentService.cs
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
    public interface IDepartmentService
    {
        Task<PagedResponse<DepartmentDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<DepartmentDto> GetByIdAsync(Guid id);
        Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
        Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<DepartmentDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Department>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.Name.Contains(search) ||
                    (d.Type != null && d.Type.Contains(search))
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

            var dtoItems = _mapper.Map<List<DepartmentDto>>(items);

            return new PagedResponse<DepartmentDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<DepartmentDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            var entity = _mapper.Map<Department>(dto);
            await _unitOfWork.Repository<Department>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto dto)
        {
            var entity = await _unitOfWork.Repository<Department>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Department not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Department>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<DepartmentDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Department>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
