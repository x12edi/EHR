using EHR.Domain.Entities;

namespace EHR.Infrastructure.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByMrnAsync(string mrn);
    }
}
