using System.Threading.Tasks;

namespace EHR.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> CommitAsync();
    }
}
