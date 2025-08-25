using EHR.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace EHR.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IAllergyRepository Allergies { get; }
        //IAppointmentRepository Appointments { get; }
        //IAuditAccessLogRepository AuditAccessLogs { get; }
        //IAuditLogRepository AuditLogs { get; }
        //IBillingRecordRepository BillingRecords { get; }
        //IClinicalNoteRepository ClinicalNotes { get; }
        //IClinicianProfileRepository ClinicianProfiles { get; }
        //ICodeSetItemRepository CodeSetItems { get; }
        //ICodeSetRepository CodeSets { get; }
        //IContactRepository Contacts { get; }
        //IDepartmentRepository Departments { get; }   
        //IDiagnosticReportRepository Diagnostics { get; }
        //IEncounterParticipantRepository EncounterParticipants { get; }
        //IEncounterRepository Encounters { get; }
        //IFileReferenceRepository FileReferences { get; }
        //IImagingReferenceRepository ImagingReferences { get; }
        //ILabResultRepository LabResults { get; }
        
        //ILocationRepository Locations { get; }
        //IMedicationAdministrationRepository MedicationAdministrations { get; }
        //IMedicationRequestRepository MedicationRequests { get; }
        //IOrderRepository Orders { get; }
        //IOutboxEventRepository OutboxEvents { get; }
        //IPatientIdentifierRepository PatientIdentifiers { get; }
        //IPatientRepository Patients { get; }
        //IProblemRepository Problems { get; }
        //IRefreshTokenRepository RefreshTokens { get; }
        //IScheduleSlotRepository ScheduleSlots { get; }
        //ITenantRepository Tenants { get; }
        //IUserRepository Users { get; }
        EhrDbContext Context { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> CommitAsync();

        // Transaction helpers
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
