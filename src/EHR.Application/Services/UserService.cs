// EHR.Application/Services/UserService.cs
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
    public interface IUserService
    {
        Task<PagedResponse<UserDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetBySubjectIdAsync(string subjectId);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(UpdateUserDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<UserDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<User>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search) || u.DisplayName.Contains(search));
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

            var dtoItems = _mapper.Map<List<UserDto>>(items);

            return new PagedResponse<UserDto>(dtoItems, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> GetBySubjectIdAsync(string subjectId)
        {
            var entity = await _unitOfWork.Repository<User>().Query()
                .FirstOrDefaultAsync(u => u.SubjectId == subjectId);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            await _unitOfWork.Repository<User>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> UpdateAsync(UpdateUserDto dto)
        {
            var entity = await _unitOfWork.Repository<User>().GetByIdAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("User not found.");

            _mapper.Map(dto, entity);
            _unitOfWork.Repository<User>().Update(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<User>().Remove(entity);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
