using AutoMapper;
using EHR.Domain.Entities; // or whatever your entity namespace is
using EHR.Application.DTOs;

namespace EHR.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Replace with your real entity/DTO names
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();
        }
    }
}
