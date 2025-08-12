using EHR.Domain.Entities;
using EHR.Infrastructure.Persistence;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EHR.Infrastructure.Repositories.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(EhrDbContext context) : base(context) { }

        public async Task<Patient?> GetByMrnAsync(string mrn)
        {
            if (string.IsNullOrWhiteSpace(mrn)) return null;
            return await _dbSet.FirstOrDefaultAsync(p => p.MRN == mrn);
        }
    }
}
