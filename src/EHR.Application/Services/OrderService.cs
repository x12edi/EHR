// EHR.Application/Services/OrderService.cs
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
    public interface IOrderService
    {
        Task<PagedResponse<OrderDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<OrderDto> GetByIdAsync(Guid id);
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateAsync(UpdateOrderDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<OrderDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Order>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o =>
                    o.OrderNumber.Contains(search) ||
                    o.OrderType.Contains(search) ||
                    o.OrderStatus.Contains(search) ||
                    o.Priority.Contains(search)
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

            var dtoItems = _mapper.Map<List<OrderDto>>(items);

            return new PagedResponse<OrderDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OrderDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var entity = _mapper.Map<Order>(dto);
            await _unitOfWork.Repository<Order>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<OrderDto> UpdateAsync(UpdateOrderDto dto)
        {
            var entity = await _unitOfWork.Repository<Order>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Order not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Order>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrderDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Order>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
