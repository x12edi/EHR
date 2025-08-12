// EHR.Application/Services/ImagingReferenceService.cs
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
    public interface IImagingReferenceService
    {
        Task<PagedResponse<ImagingReferenceDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ImagingReferenceDto> GetByIdAsync(Guid id);
        Task<ImagingReferenceDto> CreateAsync(CreateImagingReferenceDto dto);
        Task<ImagingReferenceDto> UpdateAsync(UpdateImagingReferenceDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ImagingReferenceService : IImagingReferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImagingReferenceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ImagingReferenceDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<ImagingReference>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i =>
                    i.Modality.Contains(search) ||
                    i.AccessionNumber.Contains(search) ||
                    i.StudyInstanceUID.Contains(search) ||
                    i.SeriesInstanceUID.Contains(search) ||
                    i.SOPInstanceUID.Contains(search)
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

            var dtoItems = _mapper.Map<List<ImagingReferenceDto>>(items);

            return new PagedResponse<ImagingReferenceDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ImagingReferenceDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ImagingReference>().GetByIdAsync(id);
            return _mapper.Map<ImagingReferenceDto>(entity);
        }

        public async Task<ImagingReferenceDto> CreateAsync(CreateImagingReferenceDto dto)
        {
            var entity = _mapper.Map<ImagingReference>(dto);
            await _unitOfWork.Repository<ImagingReference>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ImagingReferenceDto>(entity);
        }

        public async Task<ImagingReferenceDto> UpdateAsync(UpdateImagingReferenceDto dto)
        {
            var entity = await _unitOfWork.Repository<ImagingReference>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("ImagingReference not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<ImagingReference>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ImagingReferenceDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ImagingReference>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<ImagingReference>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
