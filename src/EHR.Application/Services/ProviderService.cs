// Application/Services/ProviderService.cs
using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Repositories.Interfaces;
using EHR.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EHR.Application.Services
{
    public interface IProviderService
    {
        Task<PagedResponse<ProviderDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<ProviderDto> GetByIdAsync(Guid id);
        Task<ProviderDto> CreateAsync(CreateProviderDto dto, string currentUser);
        Task<ProviderDto> UpdateAsync(UpdateProviderDto dto, string currentUser);
        Task<bool> DeleteAsync(Guid id);
    }

    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProviderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProviderDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true)
        {
            var query = _unitOfWork.Repository<ClinicianProfile>()
                .Query();
                //.Include(c => c.User);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.User.DisplayName.Contains(search) ||
                    c.NPI.Contains(search) ||
                    c.LicenseNumber.Contains(search) ||
                    c.SpecialtyCode.Contains(search)
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
                .Select(c => new ProviderDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Username = c.User.Username,
                    Email = c.User.Email,
                    DisplayName = c.User.DisplayName,
                    IsActive = c.User.IsActive,
                    NPI = c.NPI,
                    LicenseNumber = c.LicenseNumber,
                    SpecialtyCode = c.SpecialtyCode,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.Department != null ? c.Department.Name : null,
                    ContactJson = c.ContactJson
                })
                .ToListAsync(); 

            //var dtoItems = items.Select(c => new ProviderDto
            //{
            //    Id = c.Id,
            //    UserId = c.UserId,
            //    Username = c.User.Username,
            //    Email = c.User.Email,
            //    DisplayName = c.User.DisplayName,
            //    IsActive = c.User.IsActive,
            //    NPI = c.NPI,
            //    LicenseNumber = c.LicenseNumber,
            //    SpecialtyCode = c.SpecialtyCode,
            //    DepartmentId = c.DepartmentId,
            //    ContactJson = c.ContactJson
            //}).ToList();

            return new PagedResponse<ProviderDto>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<ProviderDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<ClinicianProfile>()
                .Query()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null) throw new KeyNotFoundException("Provider not found");

            return new ProviderDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Username = entity.User.Username,
                Email = entity.User.Email,
                DisplayName = entity.User.DisplayName,
                IsActive = entity.User.IsActive,
                NPI = entity.NPI,
                LicenseNumber = entity.LicenseNumber,
                SpecialtyCode = entity.SpecialtyCode,
                DepartmentId = entity.DepartmentId,
                ContactJson = entity.ContactJson
            };
        }

        public async Task<ProviderDto> CreateAsync(CreateProviderDto dto, string currentUser)
        {
            var strategy = _unitOfWork.Context.Database.CreateExecutionStrategy(); // you need to expose Context

            return await strategy.ExecuteAsync(async () =>
            {
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = dto.Username,
                        Email = dto.Email,
                        DisplayName = dto.DisplayName,
                        IsSystemAccount = false,
                        Locale = "en-US",
                        TimeZone = "UTC",
                        ProfileJson = "{}",
                        IsActive = true,
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = currentUser
                    };

                    await _unitOfWork.Repository<User>().AddAsync(user);
                    await _unitOfWork.CommitAsync();

                    var clinician = new ClinicianProfile
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        NPI = dto.NPI,
                        LicenseNumber = dto.LicenseNumber,
                        SpecialtyCode = dto.SpecialtyCode,
                        DepartmentId = dto.DepartmentId,
                        ContactJson = dto.ContactJson ?? "{}",
                        IsActive = true,
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = currentUser
                    };

                    await _unitOfWork.Repository<ClinicianProfile>().AddAsync(clinician);
                    await _unitOfWork.CommitAsync();

                    await _unitOfWork.CommitTransactionAsync();

                    return new ProviderDto
                    {
                        Id = clinician.Id,
                        UserId = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        IsActive = user.IsActive,
                        NPI = clinician.NPI,
                        LicenseNumber = clinician.LicenseNumber,
                        SpecialtyCode = clinician.SpecialtyCode,
                        DepartmentId = clinician.DepartmentId,
                        ContactJson = clinician.ContactJson
                    };
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            });
        }


        public async Task<ProviderDto> UpdateAsync(UpdateProviderDto dto, string currentUser)
        {
            var strategy = _unitOfWork.Context.Database.CreateExecutionStrategy(); // you need to expose Context
            return await strategy.ExecuteAsync(async () =>
            {


                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);
                    if (user == null) throw new KeyNotFoundException("User not found");

                    //user.Username = dto.Username;
                    user.Email = dto.Email;
                    user.DisplayName = dto.DisplayName;
                    user.UpdatedAt = DateTimeOffset.UtcNow;
                    user.UpdatedBy = currentUser;

                    _unitOfWork.Repository<User>().Update(user);

                    var clinician = await _unitOfWork.Repository<ClinicianProfile>().GetByIdAsync(dto.Id);
                    if (clinician == null) throw new KeyNotFoundException("Clinician profile not found");

                    clinician.NPI = dto.NPI;
                    clinician.LicenseNumber = dto.LicenseNumber;
                    clinician.SpecialtyCode = dto.SpecialtyCode;
                    clinician.DepartmentId = dto.DepartmentId;
                    clinician.ContactJson = dto.ContactJson;
                    clinician.UpdatedAt = DateTimeOffset.UtcNow;
                    clinician.UpdatedBy = currentUser;

                    _unitOfWork.Repository<ClinicianProfile>().Update(clinician);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    return await GetByIdAsync(dto.Id);
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            });
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var clinician = await _unitOfWork.Repository<ClinicianProfile>().GetByIdAsync(id);
                if (clinician == null) return false;

                var user = await _unitOfWork.Repository<User>().GetByIdAsync(clinician.UserId);

                _unitOfWork.Repository<ClinicianProfile>().Remove(clinician);
                if (user != null) _unitOfWork.Repository<User>().Remove(user);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
