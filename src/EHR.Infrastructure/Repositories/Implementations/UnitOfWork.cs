using EHR.Infrastructure.Persistence;
using EHR.Infrastructure.Repositories.Interfaces;

namespace EHR.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EhrDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public IPatientRepository Patients { get; }

        public UnitOfWork(EhrDbContext context,
                          IPatientRepository patientRepository,
                          IServiceProvider serviceProvider)
        {
            _context = context;
            Patients = patientRepository;
            _serviceProvider = serviceProvider;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return (IRepository<T>)_serviceProvider.GetService(typeof(IRepository<T>))!;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
