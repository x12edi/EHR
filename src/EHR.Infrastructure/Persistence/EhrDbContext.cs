
using EHR.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Reflection.Emit;
using EHR.Domain.Common;
using EHR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EHR.Infrastructure.Persistence
{
    public class EhrDbContext : DbContext
    {
        public EhrDbContext(DbContextOptions<EhrDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientIdentifier> PatientIdentifiers { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<ClinicianProfile> ClinicianProfiles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<EncounterParticipant> EncounterParticipants { get; set; }

        public DbSet<ClinicalNote> ClinicalNotes { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Allergy> Allergies { get; set; }

        public DbSet<MedicationRequest> MedicationRequests { get; set; }
        public DbSet<MedicationAdministration> MedicationAdministrations { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<DiagnosticReport> DiagnosticReports { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ScheduleSlot> ScheduleSlots { get; set; }

        public DbSet<BillingRecord> BillingRecords { get; set; }

        public DbSet<FileReference> FileReferences { get; set; }
        public DbSet<ImagingReference> ImagingReferences { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditAccessLog> AuditAccessLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<CodeSet> CodeSets { get; set; }
        public DbSet<CodeSetItem> CodeSetItems { get; set; }

        public DbSet<OutboxEvent> OutboxEvents { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply common config for AuditableEntity types
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(t => typeof(AuditableEntity).IsAssignableFrom(t.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(AuditableEntity.RowVersion))
                    .IsRowVersion();

                // Global query filter for soft-delete (IsActive)
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(CreateIsActiveFilter(entityType.ClrType));
            }

            // Patients
            modelBuilder.Entity<Patient>(b =>
            {
                b.ToTable("Patients");
                b.HasKey(p => p.Id);
                b.HasIndex(p => p.MRN).IsUnique();
                b.HasIndex(p => new { p.LastName, p.FirstName, p.DOB });
                b.Property(p => p.AddressesJson).HasColumnType("nvarchar(max)");
                b.Property(p => p.IdentifiersJson).HasColumnType("nvarchar(max)");
                b.Property(p => p.DemographicsJson).HasColumnType("nvarchar(max)");
                b.Property(p => p.Email).HasMaxLength(256);
                b.Property(p => p.PrimaryPhone).HasMaxLength(50);
                // optionally create computed column or trigger to populate FullNameNormalized for FTS
                b.HasMany(p => p.Encounters).WithOne(e => e.Patient).HasForeignKey(e => e.PatientId);
            });

            // PatientIdentifiers
            modelBuilder.Entity<PatientIdentifier>(b =>
            {
                b.ToTable("PatientIdentifiers");
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.IdentifierType, x.Value });
                b.HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId);
            });

            // Contact
            modelBuilder.Entity<Contact>(b =>
            {
                b.ToTable("Contacts");
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId);
            });

            // Users (profile extension)
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Username);
                b.HasIndex(u => u.Email);
                b.Property(u => u.ProfileJson).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<ClinicianProfile>(b =>
            {
                b.ToTable("ClinicianProfiles");
                b.HasKey(cp => cp.Id);
                b.HasIndex(cp => cp.NPI);
                b.HasOne(cp => cp.User).WithOne(u => u.ClinicianProfile).HasForeignKey<ClinicianProfile>(cp => cp.UserId);
                b.HasOne(cp => cp.Department).WithMany(d => d.Clinicians).HasForeignKey(cp => cp.DepartmentId).OnDelete(DeleteBehavior.Restrict);
            });

            // Department, Location
            modelBuilder.Entity<Department>(b =>
            {
                b.ToTable("Departments");
                b.HasKey(d => d.Id);
                b.Property(d => d.AddressJson).HasColumnType("nvarchar(max)");
            });
            modelBuilder.Entity<Location>(b =>
            {
                b.ToTable("Locations");
                b.HasKey(l => l.Id);
            });

            // Encounters
            modelBuilder.Entity<Encounter>(b =>
            {
                b.ToTable("Encounters");
                b.HasKey(e => e.Id);
                b.HasIndex(e => new { e.PatientId, e.StartAt });
                b.HasIndex(e => e.EncounterNumber).IsUnique(false);
                b.HasOne(e => e.Patient).WithMany(p => p.Encounters).HasForeignKey(e => e.PatientId);
                b.HasOne(e => e.Location).WithMany(l => l.Encounters).HasForeignKey(e => e.LocationId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(e => e.Department).WithMany(d => d.Encounters).HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(e => e.PrimaryProvider).WithMany().HasForeignKey(e => e.PrimaryProviderId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EncounterParticipant>(b =>
            {
                b.ToTable("EncounterParticipants");
                b.HasKey(ep => ep.Id);
                b.HasOne(ep => ep.Encounter).WithMany(e => e.Participants).HasForeignKey(ep => ep.EncounterId);
                b.HasOne(ep => ep.User).WithMany().HasForeignKey(ep => ep.UserId);
            });

            // ClinicalNotes
            modelBuilder.Entity<ClinicalNote>(b =>
            {
                b.ToTable("ClinicalNotes");
                b.HasKey(n => n.Id);
                b.HasIndex(n => new { n.PatientId, n.CreatedAt });
                b.Property(n => n.Content).HasColumnType("nvarchar(max)");
                b.Property(n => n.StructuredDataJson).HasColumnType("nvarchar(max)");
                b.HasOne(n => n.Patient).WithMany(p => p.ClinicalNotes).HasForeignKey(n => n.PatientId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
                b.HasOne(n => n.Author).WithMany(u => u.ClinicalNotesAuthored).HasForeignKey(n => n.AuthorId).OnDelete(DeleteBehavior.Restrict);
            });

            // Problems
            modelBuilder.Entity<Problem>(b =>
            {
                b.ToTable("Problems");
                b.HasKey(p => p.Id);
                b.HasIndex(p => new { p.PatientId, p.Status });
            });

            // Allergies
            modelBuilder.Entity<Allergy>(b =>
            {
                b.ToTable("Allergies");
                b.HasKey(a => a.Id);
                b.HasIndex(a => new { a.PatientId, a.Status });
            });

            // MedicationRequest & administration
            modelBuilder.Entity<MedicationRequest>(b =>
            {
                b.ToTable("MedicationRequests");
                b.HasKey(m => m.Id);
                b.HasIndex(m => new { m.PatientId, m.Status });
                b.HasOne(m => m.Patient).WithMany(p => p.MedicationRequests).HasForeignKey(m => m.PatientId);
                b.HasOne(m => m.Prescriber).WithMany().HasForeignKey(m => m.PrescriberId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MedicationAdministration>(b =>
            {
                b.ToTable("MedicationAdministrations");
                b.HasKey(ma => ma.Id);
                b.HasOne(ma => ma.MedicationRequest).WithMany(m => m.Administrations).HasForeignKey(ma => ma.MedicationRequestId);
                b.HasOne(ma => ma.AdministeredBy).WithMany().HasForeignKey(ma => ma.AdministeredById).OnDelete(DeleteBehavior.Restrict);
            });

            // Orders, LabResults, DiagnosticReports
            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable("Orders");
                b.HasKey(o => o.Id);
                b.HasIndex(o => new { o.PatientId, o.OrderType });
                b.HasOne(o => o.Patient).WithMany().HasForeignKey(o => o.PatientId);
                b.HasOne(o => o.Encounter).WithMany(e => e.Orders).HasForeignKey(o => o.EncounterId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(o => o.OrderedBy).WithMany().HasForeignKey(o => o.OrderedById).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LabResult>(b =>
            {
                b.ToTable("LabResults");
                b.HasKey(lr => lr.Id);
                b.HasIndex(lr => new { lr.PatientId, lr.TestCode, lr.ResultedAt });
                b.Property(lr => lr.RawJson).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<DiagnosticReport>(b =>
            {
                b.ToTable("DiagnosticReports");
                b.HasKey(dr => dr.Id);
                b.Property(dr => dr.ReportJson).HasColumnType("nvarchar(max)");
            });

            // Appointments & schedule
            modelBuilder.Entity<Appointment>(b =>
            {
                b.ToTable("Appointments");
                b.HasKey(a => a.Id);
                b.HasIndex(a => new { a.ProviderId, a.StartAt });
                b.HasOne(a => a.Patient).WithMany(p => p.Appointments).HasForeignKey(a => a.PatientId);
                b.HasOne(a => a.Provider).WithMany().HasForeignKey(a => a.ProviderId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ScheduleSlot>(b =>
            {
                b.ToTable("ScheduleSlots");
                b.HasKey(s => s.Id);
                b.HasIndex(s => new { s.ProviderId, s.StartAt });
            });

            // Billing
            modelBuilder.Entity<BillingRecord>(b =>
            {
                b.ToTable("BillingRecords");
                b.HasKey(bi => bi.Id);
                b.HasOne(bi => bi.Patient).WithMany().HasForeignKey(bi => bi.PatientId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(bi => bi.Encounter).WithMany().HasForeignKey(bi => bi.EncounterId).OnDelete(DeleteBehavior.Restrict);
            });

            // Files & imaging
            modelBuilder.Entity<FileReference>(b =>
            {
                b.ToTable("FileReferences");
                b.HasKey(f => f.Id);
                b.HasIndex(f => new { f.EntityType, f.EntityId });
                b.Property(f => f.MetadataJson).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<ImagingReference>(b =>
            {
                b.ToTable("ImagingReferences");
                b.HasKey(i => i.Id);
                b.HasIndex(i => new { i.StudyInstanceUID, i.SeriesInstanceUID, i.SOPInstanceUID });
                b.HasOne(i => i.Encounter).WithMany(e => e.ImagingReferences).HasForeignKey(i => i.EncounterId).OnDelete(DeleteBehavior.Restrict);
            });

            // Audits & notifications
            modelBuilder.Entity<AuditLog>(b =>
            {
                b.ToTable("AuditLogs");
                b.HasKey(a => a.Id);
                b.HasIndex(a => new { a.EntityType, a.EntityId });
                b.Property(a => a.DetailsJson).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<AuditAccessLog>(b =>
            {
                b.ToTable("AuditAccessLogs");
                b.HasKey(a => a.Id);
                b.HasIndex(a => new { a.UserId, a.PatientId, a.Timestamp });
            });

            modelBuilder.Entity<Notification>(b =>
            {
                b.ToTable("Notifications");
                b.HasKey(n => n.Id);
                b.HasIndex(n => n.RecipientUserId);
            });

            // Code sets & outbox
            modelBuilder.Entity<CodeSet>(b =>
            {
                b.ToTable("CodeSets");
                b.HasKey(c => c.Id);
            });
            modelBuilder.Entity<CodeSetItem>(b =>
            {
                b.ToTable("CodeSetItems");
                b.HasKey(ci => ci.Id);
                b.HasIndex(ci => new { ci.CodeSetId, ci.ItemCode });
            });

            modelBuilder.Entity<OutboxEvent>(b =>
            {
                b.ToTable("OutboxEvents");
                b.HasKey(o => o.Id);
                b.HasIndex(o => o.Status);
            });

            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.ToTable("RefreshTokens");
                b.HasKey(rt => rt.Id);
                b.HasIndex(rt => rt.UserId);
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.ToTable("Tenants");
                b.HasKey(t => t.Id);
            });

            // Additional configuration: set default schema, collations, etc as needed.
            // Consider creating computed column for FullNameNormalized, and create Full-Text indexes via migrations or SQL scripts.
        }

        private static LambdaExpression CreateIsActiveFilter(Type entityType)
        {
            // builds (e => EF.Property<bool>(e, "IsActive") == true)
            var param = Expression.Parameter(entityType, "e");
            var prop = Expression.Call(
                typeof(EF), nameof(EF.Property),
                new Type[] { typeof(bool) },
                param,
                Expression.Constant("IsActive"));
            var condition = Expression.Equal(prop, Expression.Constant(true));
            return Expression.Lambda(condition, param);
        }

        // optionally override SaveChanges to capture Audit logs, Outbox events etc
    }
}
