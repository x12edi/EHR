// Services/AuditLogService.cs
using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Extensions;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHR.Application.Services
{
    public class AuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<AuditLogDto>> GetAllAsync(
            PaginationParameter pagination,
            string search = null,
            string sortBy = null,
            bool isAscending = true)
        {
            var query = _unitOfWork.Repository<AuditLog>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.ActionType.Contains(search) ||
                    a.EntityType.Contains(search) ||
                    a.EntityId.Contains(search)
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

            var dtoItems = _mapper.Map<List<AuditLogDto>>(items);
            return new PagedResponse<AuditLogDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }
    }
}
