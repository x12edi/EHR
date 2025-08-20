using AutoMapper;
using EHR.Domain.Entities; // or whatever your entity namespace is
using EHR.Application.DTOs;

namespace EHR.Application.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            // Replace with your real entity/DTO names
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();
        }
    }
}
