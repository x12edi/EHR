// EHR.Application/Services/ContactService.cs
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
    public interface IContactService
    {
        Task<PagedResponse<ContactDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ContactDto> GetByIdAsync(Guid id);
        Task<ContactDto> CreateAsync(CreateContactDto dto);
        Task<ContactDto> UpdateAsync(UpdateContactDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ContactDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Contact>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Relationship.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.Notes.Contains(search)
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

            var dtoItems = _mapper.Map<List<ContactDto>>(items);

            return new PagedResponse<ContactDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ContactDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task<ContactDto> CreateAsync(CreateContactDto dto)
        {
            var entity = _mapper.Map<Contact>(dto);
            await _unitOfWork.Repository<Contact>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task<ContactDto> UpdateAsync(UpdateContactDto dto)
        {
            var entity = await _unitOfWork.Repository<Contact>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Contact not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<Contact>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ContactDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<Contact>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
