using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Extensions;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EHR.Application.Services
{
    public interface IBillingRecordService
    {
        Task<PagedResponse<BillingRecordDto>> GetAllAsync(
            PaginationParameter pagination,
            string search = null,
            string sortBy = null,
            bool isAscending = true);

        Task<BillingRecordDto> GetByIdAsync(Guid id);
        Task<BillingRecordDto> CreateAsync(CreateBillingRecordDto dto);
        Task UpdateAsync(Guid id, UpdateBillingRecordDto dto);
        Task DeleteAsync(Guid id);
    }
    public class BillingRecordService : IBillingRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillingRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<BillingRecordDto>> GetAllAsync(
            PaginationParameter pagination,
            string search = null,
            string sortBy = null,
            bool isAscending = true)
        {
            var query = _unitOfWork.Repository<BillingRecord>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(br =>
                    br.Status.Contains(search) ||
                    br.Currency.Contains(search));
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

            var dtoItems = _mapper.Map<List<BillingRecordDto>>(items);

            return new PagedResponse<BillingRecordDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<BillingRecordDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<BillingRecord>().GetByIdAsync(id);
            return _mapper.Map<BillingRecordDto>(entity);
        }

        public async Task<BillingRecordDto> CreateAsync(CreateBillingRecordDto dto)
        {
            var entity = _mapper.Map<BillingRecord>(dto);
            await _unitOfWork.Repository<BillingRecord>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<BillingRecordDto>(entity);
        }

        public async Task UpdateAsync(Guid id, UpdateBillingRecordDto dto)
        {
            var entity = await _unitOfWork.Repository<BillingRecord>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Billing record not found");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<BillingRecord>().Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<BillingRecord>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Billing record not found");

            _unitOfWork.Repository<BillingRecord>().Remove(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
