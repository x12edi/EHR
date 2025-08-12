// EHR.Application/Services/LocationService.cs
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
    public interface ILocationService
    {
        Task<PagedResponse<LocationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<LocationDto> GetByIdAsync(Guid id);
        Task<LocationDto> CreateAsync(CreateLocationDto dto);
        Task<LocationDto> UpdateAsync(UpdateLocationDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<LocationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Location>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(l =>
                    l.Name.Contains(search) ||
                    l.Code.Contains(search) ||
                    (l.Type != null && l.Type.Contains(search))
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

            var dtoItems = _mapper.Map<List<LocationDto>>(items);

            return new PagedResponse<LocationDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<LocationDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
            return _mapper.Map<LocationDto>(entity);
        }

        public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
        {
            var entity = _mapper.Map<Location>(dto);
            await _unitOfWork.Repository<Location>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationDto>(entity);
        }

        public async Task<LocationDto> UpdateAsync(UpdateLocationDto dto)
        {
            var entity = await _unitOfWork.Repository<Location>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Location not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Location>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<LocationDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Location>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
