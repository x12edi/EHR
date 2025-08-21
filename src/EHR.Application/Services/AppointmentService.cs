using AutoMapper;
using EHR.Application.DTOs;
using EHR.Application.Interfaces;
using EHR.Application.Parameters;
using EHR.Application.Wrappers;
using EHR.Domain.Entities;
using EHR.Infrastructure.Extensions;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EHR.Application.Services
{
    public interface IAppointmentService
    {
        Task<PagedResponse<AppointmentDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<AppointmentDto> GetByIdAsync(Guid id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task UpdateAsync(Guid id, UpdateAppointmentDto dto);
        Task DeleteAsync(Guid id);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor http)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _http = http;
        }

        private string? CurrentUserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //public async Task<PagedResponse<AppointmentDto>> GetAllAsync(PaginationParameter pagination, string search = null,   string sortBy = null,    bool isAscending = true)
        //{
        //    var query = _unitOfWork.Repository<Appointment>().Query();

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(a =>
        //            a.Status.Contains(search) ||
        //            a.CancelReason.Contains(search)
        //        );
        //    }

        //    if (!string.IsNullOrEmpty(sortBy))
        //    {
        //        query = isAscending
        //            ? query.OrderByDynamic(sortBy, true)
        //            : query.OrderByDynamic(sortBy, false);
        //    }

        //    var totalCount = await query.CountAsync();
        //    var items = await query
        //        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
        //        .Take(pagination.PageSize)
        //        .Select(a => new AppointmentDto
        //        {
        //            Id = a.Id,
        //            PatientId = a.PatientId,
        //            ProviderId = a.ProviderId,
        //            DepartmentId = a.DepartmentId,
        //            StartAt = a.StartAt,
        //            EndAt = a.EndAt,
        //            Status = a.Status,
        //            CancelReason = a.CancelReason,
        //            CheckInAt = a.CheckInAt,
        //            CheckOutAt = a.CheckOutAt
        //        })
        //        .ToListAsync();

        //    return new PagedResponse<AppointmentDto>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        //}

        public async Task<PagedResponse<AppointmentDto>> GetAllAsync(PaginationParameter pagination,string? search = null,string? sortBy = null,bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Appointment>().Query();
                
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.Status.Contains(search) ||
                    a.CancelReason.Contains(search) ||
                    a.Patient.FirstName.Contains(search) ||
                    a.Patient.LastName.Contains(search) ||
                    a.Department.Name.Contains(search) ||
                    a.Provider.DisplayName.Contains(search)
                );
            }

            // Sorting (example)
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
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                    ProviderId = a.ProviderId,
                    ProviderName = a.Provider != null ? a.Provider.DisplayName : null,
                    DepartmentId = a.DepartmentId,
                    DepartmentName = a.Department != null ? a.Department.Name : null,
                    StartAt = a.StartAt,
                    EndAt = a.EndAt,
                    Status = a.Status,
                    CancelReason = a.CancelReason,
                    CheckInAt = a.CheckInAt,
                    CheckOutAt = a.CheckOutAt
                })
                .ToListAsync();

            return new PagedResponse<AppointmentDto>(
                items,
                totalCount,
                pagination.PageNumber,
                pagination.PageSize
            );
        }


        public async Task<AppointmentDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            return _mapper.Map<AppointmentDto>(entity);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var entity = _mapper.Map<Appointment>(dto);
            entity.ProviderId = Guid.Parse(CurrentUserId);
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = CurrentUserId;

            await _unitOfWork.Repository<Appointment>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AppointmentDto>(entity);
        }

        public async Task UpdateAsync(Guid id, UpdateAppointmentDto dto)
        {
            var entity = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            if (entity == null) throw new Exception("Appointment not found");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = CurrentUserId;

            _unitOfWork.Repository<Appointment>().Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            if (entity == null) throw new Exception("Appointment not found");

            _unitOfWork.Repository<Appointment>().Remove(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
