// EHR.Application/Services/ClinicalNoteService.cs
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
    public interface IClinicalNoteService
    {
        Task<PagedResponse<ClinicalNoteDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ClinicalNoteDto> GetByIdAsync(Guid id);
        Task<ClinicalNoteDto> CreateAsync(CreateClinicalNoteDto dto);
        Task<ClinicalNoteDto> UpdateAsync(UpdateClinicalNoteDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ClinicalNoteService : IClinicalNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClinicalNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ClinicalNoteDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<ClinicalNote>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n =>
                    n.NoteType.Contains(search) ||
                    n.Content.Contains(search)
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

            var dtoItems = _mapper.Map<List<ClinicalNoteDto>>(items);

            return new PagedResponse<ClinicalNoteDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ClinicalNoteDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ClinicalNote>().GetByIdAsync(id);
            return _mapper.Map<ClinicalNoteDto>(entity);
        }

        public async Task<ClinicalNoteDto> CreateAsync(CreateClinicalNoteDto dto)
        {
            var entity = _mapper.Map<ClinicalNote>(dto);
            await _unitOfWork.Repository<ClinicalNote>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ClinicalNoteDto>(entity);
        }

        public async Task<ClinicalNoteDto> UpdateAsync(UpdateClinicalNoteDto dto)
        {
            var entity = await _unitOfWork.Repository<ClinicalNote>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Clinical note not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<ClinicalNote>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ClinicalNoteDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ClinicalNote>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<ClinicalNote>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
