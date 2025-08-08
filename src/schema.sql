IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AuditAccessLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] uniqueidentifier NULL,
    [PatientId] uniqueidentifier NULL,
    [Action] nvarchar(max) NOT NULL,
    [ResourceType] nvarchar(max) NOT NULL,
    [ResourceId] nvarchar(max) NOT NULL,
    [PurposeOfUse] nvarchar(max) NOT NULL,
    [Timestamp] datetimeoffset NOT NULL,
    [SourceIp] nvarchar(max) NOT NULL,
    [CorrelationId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AuditAccessLogs] PRIMARY KEY ([Id])
);

CREATE TABLE [CodeSets] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Version] nvarchar(max) NOT NULL,
    [MetadataJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_CodeSets] PRIMARY KEY ([Id])
);

CREATE TABLE [Departments] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [AddressJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);

CREATE TABLE [Notifications] (
    [Id] uniqueidentifier NOT NULL,
    [RecipientUserId] uniqueidentifier NULL,
    [Title] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [PayloadJson] nvarchar(max) NOT NULL,
    [IsRead] bit NOT NULL,
    [Severity] nvarchar(max) NOT NULL,
    [SentAt] datetimeoffset NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
);

CREATE TABLE [OutboxEvents] (
    [Id] bigint NOT NULL IDENTITY,
    [EventType] nvarchar(max) NOT NULL,
    [PayloadJson] nvarchar(max) NOT NULL,
    [Status] nvarchar(450) NOT NULL,
    [Retries] int NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [SentAt] datetimeoffset NULL,
    CONSTRAINT [PK_OutboxEvents] PRIMARY KEY ([Id])
);

CREATE TABLE [Patients] (
    [Id] uniqueidentifier NOT NULL,
    [MRN] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(450) NOT NULL,
    [LastName] nvarchar(450) NOT NULL,
    [MiddleName] nvarchar(max) NOT NULL,
    [FullNameNormalized] nvarchar(max) NOT NULL,
    [DOB] datetime2 NULL,
    [Gender] nvarchar(max) NOT NULL,
    [PrimaryPhone] nvarchar(50) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [PrimaryLanguage] nvarchar(max) NOT NULL,
    [PhotoUrl] nvarchar(max) NOT NULL,
    [AddressesJson] nvarchar(max) NOT NULL,
    [IdentifiersJson] nvarchar(max) NOT NULL,
    [DemographicsJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
);

CREATE TABLE [RefreshTokens] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] uniqueidentifier NULL,
    [TokenHash] nvarchar(max) NOT NULL,
    [ExpiresAt] datetimeoffset NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [RevokedAt] datetimeoffset NULL,
    [DeviceInfo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id])
);

CREATE TABLE [Tenants] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [DataIsolationMode] nvarchar(max) NOT NULL,
    [ConfigJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [Username] nvarchar(450) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [DisplayName] nvarchar(max) NOT NULL,
    [IsSystemAccount] bit NOT NULL,
    [Locale] nvarchar(max) NOT NULL,
    [TimeZone] nvarchar(max) NOT NULL,
    [ProfileJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [CodeSetItems] (
    [Id] uniqueidentifier NOT NULL,
    [CodeSetId] uniqueidentifier NOT NULL,
    [ItemCode] nvarchar(450) NOT NULL,
    [Display] nvarchar(max) NOT NULL,
    [System] nvarchar(max) NOT NULL,
    [EffectiveFrom] datetimeoffset NULL,
    [EffectiveTo] datetimeoffset NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_CodeSetItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CodeSetItems_CodeSets_CodeSetId] FOREIGN KEY ([CodeSetId]) REFERENCES [CodeSets] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Locations] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Code] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [AddressJson] nvarchar(max) NOT NULL,
    [DepartmentId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Locations_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id])
);

CREATE TABLE [Allergies] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [SubstanceCode] nvarchar(max) NOT NULL,
    [SubstanceText] nvarchar(max) NOT NULL,
    [Reaction] nvarchar(max) NOT NULL,
    [Severity] nvarchar(max) NOT NULL,
    [Status] nvarchar(450) NOT NULL,
    [RecordedAt] datetimeoffset NULL,
    [RecordedBy] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Allergies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Allergies_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Contacts] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Relationship] nvarchar(max) NOT NULL,
    [PhonesJson] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NOT NULL,
    [IsEmergencyContact] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contacts_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PatientIdentifiers] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [IdentifierType] nvarchar(450) NOT NULL,
    [Value] nvarchar(450) NOT NULL,
    [Issuer] nvarchar(max) NOT NULL,
    [AssignedAt] datetimeoffset NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_PatientIdentifiers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PatientIdentifiers_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Problems] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [Code] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [OnsetDate] datetimeoffset NULL,
    [ResolvedDate] datetimeoffset NULL,
    [Status] nvarchar(450) NOT NULL,
    [Severity] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Problems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Problems_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AuditLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [UserId] uniqueidentifier NULL,
    [ActionType] nvarchar(max) NOT NULL,
    [EntityType] nvarchar(450) NOT NULL,
    [EntityId] nvarchar(450) NOT NULL,
    [Timestamp] datetimeoffset NOT NULL,
    [DetailsJson] nvarchar(max) NOT NULL,
    [SourceIp] nvarchar(max) NOT NULL,
    [CorrelationId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AuditLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [ClinicianProfiles] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [NPI] nvarchar(450) NOT NULL,
    [LicenseNumber] nvarchar(max) NOT NULL,
    [SpecialtyCode] nvarchar(max) NOT NULL,
    [DepartmentId] uniqueidentifier NULL,
    [ContactJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_ClinicianProfiles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClinicianProfiles_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClinicianProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Appointments] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [ProviderId] uniqueidentifier NULL,
    [DepartmentId] uniqueidentifier NULL,
    [StartAt] datetimeoffset NOT NULL,
    [EndAt] datetimeoffset NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CancelReason] nvarchar(max) NOT NULL,
    [CheckInAt] datetimeoffset NULL,
    [CheckOutAt] datetimeoffset NULL,
    [LocationId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Appointments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Appointments_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
    CONSTRAINT [FK_Appointments_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]),
    CONSTRAINT [FK_Appointments_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Appointments_Users_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Encounters] (
    [Id] uniqueidentifier NOT NULL,
    [EncounterNumber] nvarchar(450) NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [EncounterType] nvarchar(max) NOT NULL,
    [StartAt] datetimeoffset NULL,
    [EndAt] datetimeoffset NULL,
    [Status] nvarchar(max) NOT NULL,
    [LocationId] uniqueidentifier NULL,
    [DepartmentId] uniqueidentifier NULL,
    [PrimaryProviderId] uniqueidentifier NULL,
    [ChiefComplaint] nvarchar(max) NOT NULL,
    [VisitReasonCode] nvarchar(max) NOT NULL,
    [NotesSummary] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Encounters] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Encounters_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Encounters_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Encounters_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Encounters_Users_PrimaryProviderId] FOREIGN KEY ([PrimaryProviderId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ScheduleSlots] (
    [Id] uniqueidentifier NOT NULL,
    [ProviderId] uniqueidentifier NOT NULL,
    [StartAt] datetimeoffset NOT NULL,
    [EndAt] datetimeoffset NOT NULL,
    [IsAvailable] bit NOT NULL,
    [LocationId] uniqueidentifier NULL,
    [RecurrenceJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_ScheduleSlots] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScheduleSlots_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]),
    CONSTRAINT [FK_ScheduleSlots_Users_ProviderId] FOREIGN KEY ([ProviderId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [BillingRecords] (
    [Id] uniqueidentifier NOT NULL,
    [EncounterId] uniqueidentifier NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [PaidAt] datetimeoffset NULL,
    [PaymentReferenceJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_BillingRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BillingRecords_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_BillingRecords_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [ClinicalNotes] (
    [Id] uniqueidentifier NOT NULL,
    [EncounterId] uniqueidentifier NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [AuthorId] uniqueidentifier NOT NULL,
    [NoteType] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [StructuredDataJson] nvarchar(max) NOT NULL,
    [SignedAt] datetimeoffset NULL,
    [SignedBy] nvarchar(max) NOT NULL,
    [SignatureHash] nvarchar(max) NOT NULL,
    [IsAddendum] bit NOT NULL,
    [ParentNoteId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_ClinicalNotes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClinicalNotes_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]),
    CONSTRAINT [FK_ClinicalNotes_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ClinicalNotes_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [EncounterParticipants] (
    [Id] uniqueidentifier NOT NULL,
    [EncounterId] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [StartedAt] datetimeoffset NULL,
    [EndedAt] datetimeoffset NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_EncounterParticipants] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EncounterParticipants_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EncounterParticipants_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [MedicationRequests] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [EncounterId] uniqueidentifier NULL,
    [PrescriberId] uniqueidentifier NOT NULL,
    [MedicationCode] nvarchar(max) NOT NULL,
    [MedicationText] nvarchar(max) NOT NULL,
    [Dose] nvarchar(max) NOT NULL,
    [Frequency] nvarchar(max) NOT NULL,
    [Route] nvarchar(max) NOT NULL,
    [Duration] nvarchar(max) NOT NULL,
    [StartDate] datetimeoffset NULL,
    [EndDate] datetimeoffset NULL,
    [Status] nvarchar(450) NOT NULL,
    [Reason] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_MedicationRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MedicationRequests_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]),
    CONSTRAINT [FK_MedicationRequests_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MedicationRequests_Users_PrescriberId] FOREIGN KEY ([PrescriberId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL,
    [OrderNumber] nvarchar(max) NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [EncounterId] uniqueidentifier NULL,
    [OrderedById] uniqueidentifier NOT NULL,
    [OrderType] nvarchar(450) NOT NULL,
    [OrderStatus] nvarchar(max) NOT NULL,
    [RequestedAt] datetimeoffset NOT NULL,
    [Priority] nvarchar(max) NOT NULL,
    [DetailsJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Orders_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Orders_Users_OrderedById] FOREIGN KEY ([OrderedById]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [FileReferences] (
    [Id] uniqueidentifier NOT NULL,
    [EntityType] nvarchar(450) NOT NULL,
    [EntityId] uniqueidentifier NULL,
    [FileName] nvarchar(max) NOT NULL,
    [ContentType] nvarchar(max) NOT NULL,
    [SizeBytes] bigint NOT NULL,
    [StoragePath] nvarchar(max) NOT NULL,
    [Checksum] nvarchar(max) NOT NULL,
    [IsPrivate] bit NOT NULL,
    [MetadataJson] nvarchar(max) NOT NULL,
    [ClinicalNoteId] uniqueidentifier NULL,
    [EncounterId] uniqueidentifier NULL,
    [PatientId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_FileReferences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FileReferences_ClinicalNotes_ClinicalNoteId] FOREIGN KEY ([ClinicalNoteId]) REFERENCES [ClinicalNotes] ([Id]),
    CONSTRAINT [FK_FileReferences_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]),
    CONSTRAINT [FK_FileReferences_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id])
);

CREATE TABLE [MedicationAdministrations] (
    [Id] uniqueidentifier NOT NULL,
    [MedicationRequestId] uniqueidentifier NOT NULL,
    [AdministeredById] uniqueidentifier NOT NULL,
    [AdministeredAt] datetimeoffset NOT NULL,
    [DoseGiven] nvarchar(max) NOT NULL,
    [Route] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_MedicationAdministrations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MedicationAdministrations_MedicationRequests_MedicationRequestId] FOREIGN KEY ([MedicationRequestId]) REFERENCES [MedicationRequests] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MedicationAdministrations_Users_AdministeredById] FOREIGN KEY ([AdministeredById]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [DiagnosticReports] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NULL,
    [ReportType] nvarchar(max) NOT NULL,
    [Summary] nvarchar(max) NOT NULL,
    [ReportJson] nvarchar(max) NOT NULL,
    [IssuedAt] datetimeoffset NOT NULL,
    [AuthorId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_DiagnosticReports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DiagnosticReports_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]),
    CONSTRAINT [FK_DiagnosticReports_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DiagnosticReports_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [ImagingReferences] (
    [Id] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NULL,
    [EncounterId] uniqueidentifier NULL,
    [StudyInstanceUID] nvarchar(450) NOT NULL,
    [SeriesInstanceUID] nvarchar(450) NOT NULL,
    [SOPInstanceUID] nvarchar(450) NOT NULL,
    [Modality] nvarchar(max) NOT NULL,
    [PACSUrl] nvarchar(max) NOT NULL,
    [StudyDate] datetimeoffset NULL,
    [AccessionNumber] nvarchar(max) NOT NULL,
    [OrderId] uniqueidentifier NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_ImagingReferences] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ImagingReferences_Encounters_EncounterId] FOREIGN KEY ([EncounterId]) REFERENCES [Encounters] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ImagingReferences_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]),
    CONSTRAINT [FK_ImagingReferences_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id])
);

CREATE TABLE [LabResults] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [PatientId] uniqueidentifier NOT NULL,
    [TestCode] nvarchar(450) NOT NULL,
    [TestName] nvarchar(max) NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    [Unit] nvarchar(max) NOT NULL,
    [ReferenceRange] nvarchar(max) NOT NULL,
    [Flag] nvarchar(max) NOT NULL,
    [CollectedAt] datetimeoffset NOT NULL,
    [ResultedAt] datetimeoffset NOT NULL,
    [ReportedBy] nvarchar(max) NOT NULL,
    [RawJson] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [UpdatedAt] datetimeoffset NULL,
    [UpdatedBy] nvarchar(max) NOT NULL,
    [DeletedAt] datetimeoffset NULL,
    [DeletedBy] nvarchar(max) NOT NULL,
    [RowVersion] rowversion NOT NULL,
    CONSTRAINT [PK_LabResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LabResults_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LabResults_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Allergies_PatientId_Status] ON [Allergies] ([PatientId], [Status]);

CREATE INDEX [IX_Appointments_DepartmentId] ON [Appointments] ([DepartmentId]);

CREATE INDEX [IX_Appointments_LocationId] ON [Appointments] ([LocationId]);

CREATE INDEX [IX_Appointments_PatientId] ON [Appointments] ([PatientId]);

CREATE INDEX [IX_Appointments_ProviderId_StartAt] ON [Appointments] ([ProviderId], [StartAt]);

CREATE INDEX [IX_AuditAccessLogs_UserId_PatientId_Timestamp] ON [AuditAccessLogs] ([UserId], [PatientId], [Timestamp]);

CREATE INDEX [IX_AuditLogs_EntityType_EntityId] ON [AuditLogs] ([EntityType], [EntityId]);

CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);

CREATE INDEX [IX_BillingRecords_EncounterId] ON [BillingRecords] ([EncounterId]);

CREATE INDEX [IX_BillingRecords_PatientId] ON [BillingRecords] ([PatientId]);

CREATE INDEX [IX_ClinicalNotes_AuthorId] ON [ClinicalNotes] ([AuthorId]);

CREATE INDEX [IX_ClinicalNotes_EncounterId] ON [ClinicalNotes] ([EncounterId]);

CREATE INDEX [IX_ClinicalNotes_PatientId_CreatedAt] ON [ClinicalNotes] ([PatientId], [CreatedAt]);

CREATE INDEX [IX_ClinicianProfiles_DepartmentId] ON [ClinicianProfiles] ([DepartmentId]);

CREATE INDEX [IX_ClinicianProfiles_NPI] ON [ClinicianProfiles] ([NPI]);

CREATE UNIQUE INDEX [IX_ClinicianProfiles_UserId] ON [ClinicianProfiles] ([UserId]);

CREATE INDEX [IX_CodeSetItems_CodeSetId_ItemCode] ON [CodeSetItems] ([CodeSetId], [ItemCode]);

CREATE INDEX [IX_Contacts_PatientId] ON [Contacts] ([PatientId]);

CREATE INDEX [IX_DiagnosticReports_AuthorId] ON [DiagnosticReports] ([AuthorId]);

CREATE INDEX [IX_DiagnosticReports_OrderId] ON [DiagnosticReports] ([OrderId]);

CREATE INDEX [IX_DiagnosticReports_PatientId] ON [DiagnosticReports] ([PatientId]);

CREATE INDEX [IX_EncounterParticipants_EncounterId] ON [EncounterParticipants] ([EncounterId]);

CREATE INDEX [IX_EncounterParticipants_UserId] ON [EncounterParticipants] ([UserId]);

CREATE INDEX [IX_Encounters_DepartmentId] ON [Encounters] ([DepartmentId]);

CREATE INDEX [IX_Encounters_EncounterNumber] ON [Encounters] ([EncounterNumber]);

CREATE INDEX [IX_Encounters_LocationId] ON [Encounters] ([LocationId]);

CREATE INDEX [IX_Encounters_PatientId_StartAt] ON [Encounters] ([PatientId], [StartAt]);

CREATE INDEX [IX_Encounters_PrimaryProviderId] ON [Encounters] ([PrimaryProviderId]);

CREATE INDEX [IX_FileReferences_ClinicalNoteId] ON [FileReferences] ([ClinicalNoteId]);

CREATE INDEX [IX_FileReferences_EncounterId] ON [FileReferences] ([EncounterId]);

CREATE INDEX [IX_FileReferences_EntityType_EntityId] ON [FileReferences] ([EntityType], [EntityId]);

CREATE INDEX [IX_FileReferences_PatientId] ON [FileReferences] ([PatientId]);

CREATE INDEX [IX_ImagingReferences_EncounterId] ON [ImagingReferences] ([EncounterId]);

CREATE INDEX [IX_ImagingReferences_OrderId] ON [ImagingReferences] ([OrderId]);

CREATE INDEX [IX_ImagingReferences_PatientId] ON [ImagingReferences] ([PatientId]);

CREATE INDEX [IX_ImagingReferences_StudyInstanceUID_SeriesInstanceUID_SOPInstanceUID] ON [ImagingReferences] ([StudyInstanceUID], [SeriesInstanceUID], [SOPInstanceUID]);

CREATE INDEX [IX_LabResults_OrderId] ON [LabResults] ([OrderId]);

CREATE INDEX [IX_LabResults_PatientId_TestCode_ResultedAt] ON [LabResults] ([PatientId], [TestCode], [ResultedAt]);

CREATE INDEX [IX_Locations_DepartmentId] ON [Locations] ([DepartmentId]);

CREATE INDEX [IX_MedicationAdministrations_AdministeredById] ON [MedicationAdministrations] ([AdministeredById]);

CREATE INDEX [IX_MedicationAdministrations_MedicationRequestId] ON [MedicationAdministrations] ([MedicationRequestId]);

CREATE INDEX [IX_MedicationRequests_EncounterId] ON [MedicationRequests] ([EncounterId]);

CREATE INDEX [IX_MedicationRequests_PatientId_Status] ON [MedicationRequests] ([PatientId], [Status]);

CREATE INDEX [IX_MedicationRequests_PrescriberId] ON [MedicationRequests] ([PrescriberId]);

CREATE INDEX [IX_Notifications_RecipientUserId] ON [Notifications] ([RecipientUserId]);

CREATE INDEX [IX_Orders_EncounterId] ON [Orders] ([EncounterId]);

CREATE INDEX [IX_Orders_OrderedById] ON [Orders] ([OrderedById]);

CREATE INDEX [IX_Orders_PatientId_OrderType] ON [Orders] ([PatientId], [OrderType]);

CREATE INDEX [IX_OutboxEvents_Status] ON [OutboxEvents] ([Status]);

CREATE INDEX [IX_PatientIdentifiers_IdentifierType_Value] ON [PatientIdentifiers] ([IdentifierType], [Value]);

CREATE INDEX [IX_PatientIdentifiers_PatientId] ON [PatientIdentifiers] ([PatientId]);

CREATE INDEX [IX_Patients_LastName_FirstName_DOB] ON [Patients] ([LastName], [FirstName], [DOB]);

CREATE UNIQUE INDEX [IX_Patients_MRN] ON [Patients] ([MRN]);

CREATE INDEX [IX_Problems_PatientId_Status] ON [Problems] ([PatientId], [Status]);

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);

CREATE INDEX [IX_ScheduleSlots_LocationId] ON [ScheduleSlots] ([LocationId]);

CREATE INDEX [IX_ScheduleSlots_ProviderId_StartAt] ON [ScheduleSlots] ([ProviderId], [StartAt]);

CREATE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE INDEX [IX_Users_Username] ON [Users] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250808110318_InitialEhrSchema', N'9.0.8');

COMMIT;
GO

