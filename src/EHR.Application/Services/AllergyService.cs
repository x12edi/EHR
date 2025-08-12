// EHR.Application/Services/AllergyService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Repositories.Interfaces;
using EHR.Infrastructure.Repositories.Implementations;
using EHR.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace EHR.Application.Services
{
    public class AllergyService : IAllergyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllergyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResponse<AllergyDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Allergy>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.SubstanceText.Contains(search) || a.Reaction.Contains(search));
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
                .Select(a => new AllergyDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    SubstanceCode = a.SubstanceCode,
                    SubstanceText = a.SubstanceText,
                    Reaction = a.Reaction,
                    Severity = a.Severity,
                    Status = a.Status,
                    RecordedAt = a.RecordedAt,
                    RecordedBy = a.RecordedBy
                })
                .ToListAsync();

            return new PagedResponse<AllergyDto>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<AllergyDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);
            if (entity == null) return null;

            return new AllergyDto
            {
                Id = entity.Id,
                PatientId = entity.PatientId,
                SubstanceCode = entity.SubstanceCode,
                SubstanceText = entity.SubstanceText,
                Reaction = entity.Reaction,
                Severity = entity.Severity,
                Status = entity.Status,
                RecordedAt = entity.RecordedAt,
                RecordedBy = entity.RecordedBy
            };
        }

        public async Task<AllergyDto> CreateAsync(CreateAllergyDto dto)
        {
            var entity = new Allergy
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                SubstanceCode = dto.SubstanceCode,
                SubstanceText = dto.SubstanceText,
                Reaction = dto.Reaction,
                Severity = dto.Severity,
                Status = dto.Status,
                RecordedAt = dto.RecordedAt,
                RecordedBy = dto.RecordedBy,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = "system"
            };

            await _unitOfWork.Repository<Allergy>().AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<AllergyDto> UpdateAsync(UpdateAllergyDto dto)
        {
            var entity = await _unitOfWork.Repository<Allergy>().GetByIdAsync(dto.Id);
            if (entity == null) return null;

            entity.PatientId = dto.PatientId;
            entity.SubstanceCode = dto.SubstanceCode;
            entity.SubstanceText = dto.SubstanceText;
            entity.Reaction = dto.Reaction;
            entity.Severity = dto.Severity;
            entity.Status = dto.Status;
            entity.RecordedAt = dto.RecordedAt;
            entity.RecordedBy = dto.RecordedBy;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = "system";

            _unitOfWork.Repository<Allergy>().Update(entity);
            await _unitOfWork.CommitAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsActive = false;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            entity.DeletedBy = "system";

            _unitOfWork.Repository<Allergy>().Update(entity);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
