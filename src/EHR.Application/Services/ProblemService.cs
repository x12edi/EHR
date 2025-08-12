// EHR.Application/Services/ProblemService.cs
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
    public interface IProblemService
    {
        Task<PagedResponse<ProblemDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ProblemDto> GetByIdAsync(Guid id);
        Task<ProblemDto> CreateAsync(CreateProblemDto dto);
        Task<ProblemDto> UpdateAsync(UpdateProblemDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ProblemService : IProblemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProblemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProblemDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Problem>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.Code.Contains(search) ||
                    p.Description.Contains(search) ||
                    p.Status.Contains(search) ||
                    p.Severity.Contains(search)
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

            var dtoItems = _mapper.Map<List<ProblemDto>>(items);

            return new PagedResponse<ProblemDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ProblemDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Problem>().GetByIdAsync(id);
            return _mapper.Map<ProblemDto>(entity);
        }

        public async Task<ProblemDto> CreateAsync(CreateProblemDto dto)
        {
            var entity = _mapper.Map<Problem>(dto);
            await _unitOfWork.Repository<Problem>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProblemDto>(entity);
        }

        public async Task<ProblemDto> UpdateAsync(UpdateProblemDto dto)
        {
            var entity = await _unitOfWork.Repository<Problem>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Problem not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Problem>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProblemDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Problem>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Problem>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
