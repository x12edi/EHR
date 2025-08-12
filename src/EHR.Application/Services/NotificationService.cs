// EHR.Application/Services/NotificationService.cs
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
    public interface INotificationService
    {
        Task<PagedResponse<NotificationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<NotificationDto> GetByIdAsync(Guid id);
        Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
        Task<NotificationDto> UpdateAsync(UpdateNotificationDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<NotificationDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Notification>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n =>
                    n.Title.Contains(search) ||
                    n.Message.Contains(search) ||
                    n.Severity.Contains(search)
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

            var dtoItems = _mapper.Map<List<NotificationDto>>(items);

            return new PagedResponse<NotificationDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<NotificationDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);
            return _mapper.Map<NotificationDto>(entity);
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
        {
            var entity = _mapper.Map<Notification>(dto);
            await _unitOfWork.Repository<Notification>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<NotificationDto>(entity);
        }

        public async Task<NotificationDto> UpdateAsync(UpdateNotificationDto dto)
        {
            var entity = await _unitOfWork.Repository<Notification>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Notification not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Notification>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<NotificationDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Notification>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
