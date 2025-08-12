using EHR.Infrastructure.Persistence;
using EHR.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace EHR.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EhrDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction? _currentTransaction;
        //public IAllergyRepository Allergies { get; }
        //public IAppointmentRepository Appointments { get; }
        //public IAuditAccessLogRepository AuditAccessLogs { get; }
        //public IAuditLogRepository AuditLogs { get; }
        //public IBillingRecordRepository BillingRecords { get; }
        //public IClinicalNoteRepository ClinicalNotes { get; }
        //public IClinicianProfileRepository ClinicianProfiles { get; }
        //public ICodeSetItemRepository CodeSetItems { get; }
        //public ICodeSetRepository CodeSets { get; }
        //public IContactRepository Contacts { get; }
        //public IDepartmentRepository Departments { get; }
        //public IDiagnosticReportRepository Diagnostics { get; }
        //public IEncounterParticipantRepository EncounterParticipants { get; }
        //public IEncounterRepository Encounters { get; }
        //public IFileReferenceRepository FileReferences { get; }
        //public IImagingReferenceRepository ImagingReferences { get; }
        //public ILabResultRepository LabResults { get; }

        //public ILocationRepository Locations { get; }
        //public IMedicationAdministrationRepository MedicationAdministrations { get; }
        //public IMedicationRequestRepository MedicationRequests { get; }
        //public IOrderRepository Orders { get; }
        //public IOutboxEventRepository OutboxEvents { get; }
        //public IPatientIdentifierRepository PatientIdentifiers { get; }
        //public IPatientRepository Patients { get; }
        //public IProblemRepository Problems { get; }
        //public IRefreshTokenRepository RefreshTokens { get; }
        //public IScheduleSlotRepository ScheduleSlots { get; }
        //public ITenantRepository Tenants { get; }
        //public IUserRepository Users { get; }

        public UnitOfWork(EhrDbContext context,IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var repo = (IRepository<T>?)_serviceProvider.GetService(typeof(IRepository<T>));
            if (repo != null) return repo;
            // fallback to new generic repository if DI was not registered for that T
            return new Repository<T>(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                if (_currentTransaction != null) await _currentTransaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
