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
using System.Linq;
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

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<AppointmentDto>> GetAllAsync(PaginationParameter pagination, string search = null,   string sortBy = null,    bool isAscending = true)
        {
            var query = _unitOfWork.Repository<Appointment>().Query();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.Status.Contains(search) ||
                    a.CancelReason.Contains(search)
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
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    ProviderId = a.ProviderId,
                    DepartmentId = a.DepartmentId,
                    StartAt = a.StartAt,
                    EndAt = a.EndAt,
                    Status = a.Status,
                    CancelReason = a.CancelReason,
                    CheckInAt = a.CheckInAt,
                    CheckOutAt = a.CheckOutAt
                })
                .ToListAsync();

            return new PagedResponse<AppointmentDto>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }


        public async Task<AppointmentDto> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            return _mapper.Map<AppointmentDto>(entity);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var entity = _mapper.Map<Appointment>(dto);
            await _unitOfWork.Repository<Appointment>().AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<AppointmentDto>(entity);
        }

        public async Task UpdateAsync(Guid id, UpdateAppointmentDto dto)
        {
            var entity = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            if (entity == null) throw new Exception("Appointment not found");

            _mapper.Map(dto, entity);
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
