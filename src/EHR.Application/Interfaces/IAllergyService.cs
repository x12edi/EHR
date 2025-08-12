// EHR.Application/Interfaces/IAllergyService.cs
using System;
using System.Threading.Tasks;
using EHR.Application.DTOs;
using EHR.Application.Wrappers;
using EHR.Application.Parameters;

namespace EHR.Application.Interfaces
{
    public interface IAllergyService
    {
        Task<PagedResponse<AllergyDto>> GetAllAsync(PaginationParameter pagination, string search = null, string sortBy = null, bool isAscending = true);
        Task<AllergyDto> GetByIdAsync(Guid id);
        Task<AllergyDto> CreateAsync(CreateAllergyDto dto);
        Task<AllergyDto> UpdateAsync(UpdateAllergyDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
