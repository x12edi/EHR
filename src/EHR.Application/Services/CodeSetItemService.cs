// EHR.Application/Services/CodeSetItemService.cs
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
    public interface ICodeSetItemService
    {
        Task<PagedResponse<CodeSetItemDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<CodeSetItemDto> GetByIdAsync(Guid id);
        Task<CodeSetItemDto> CreateAsync(CreateCodeSetItemDto dto);
        Task<CodeSetItemDto> UpdateAsync(UpdateCodeSetItemDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class CodeSetItemService : ICodeSetItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CodeSetItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CodeSetItemDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<CodeSetItem>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.ItemCode.Contains(search) ||
                    c.Display.Contains(search) ||
                    c.System.Contains(search)
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

            var dtoItems = _mapper.Map<List<CodeSetItemDto>>(items);

            return new PagedResponse<CodeSetItemDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<CodeSetItemDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<CodeSetItem>().GetByIdAsync(id);
            return _mapper.Map<CodeSetItemDto>(entity);
        }

        public async Task<CodeSetItemDto> CreateAsync(CreateCodeSetItemDto dto)
        {
            var entity = _mapper.Map<CodeSetItem>(dto);
            await _unitOfWork.Repository<CodeSetItem>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CodeSetItemDto>(entity);
        }

        public async Task<CodeSetItemDto> UpdateAsync(UpdateCodeSetItemDto dto)
        {
            var entity = await _unitOfWork.Repository<CodeSetItem>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("CodeSetItem not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<CodeSetItem>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CodeSetItemDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<CodeSetItem>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<CodeSetItem>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
