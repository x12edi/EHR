// EHR.Application/Services/FileReferenceService.cs
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
    public interface IFileReferenceService
    {
        Task<PagedResponse<FileReferenceDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<FileReferenceDto> GetByIdAsync(Guid id);
        Task<FileReferenceDto> CreateAsync(CreateFileReferenceDto dto);
        Task<FileReferenceDto> UpdateAsync(UpdateFileReferenceDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class FileReferenceService : IFileReferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileReferenceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<FileReferenceDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<FileReference>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f =>
                    f.FileName.Contains(search) ||
                    f.EntityType.Contains(search) ||
                    f.ContentType.Contains(search) ||
                    f.Checksum.Contains(search)
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

            var dtoItems = _mapper.Map<List<FileReferenceDto>>(items);

            return new PagedResponse<FileReferenceDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<FileReferenceDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<FileReference>().GetByIdAsync(id);
            return _mapper.Map<FileReferenceDto>(entity);
        }

        public async Task<FileReferenceDto> CreateAsync(CreateFileReferenceDto dto)
        {
            var entity = _mapper.Map<FileReference>(dto);
            await _unitOfWork.Repository<FileReference>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<FileReferenceDto>(entity);
        }

        public async Task<FileReferenceDto> UpdateAsync(UpdateFileReferenceDto dto)
        {
            var entity = await _unitOfWork.Repository<FileReference>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("FileReference not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<FileReference>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<FileReferenceDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<FileReference>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<FileReference>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
