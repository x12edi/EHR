using EHR.API.Extensions;
using EHR.Infrastructure.Persistence;
using EHR.Infrastructure.Repositories.Implementations;
using EHR.Infrastructure.Repositories.Interfaces;
using EHR.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---- Configuration ----
var configuration = builder.Configuration;
var env = builder.Environment;

// Add DbContext (SQL Server)
builder.Services.AddDbContext<EhrDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                         sql => sql.EnableRetryOnFailure())
);

// Generic + specific repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IEncounterRepository, EncounterRepository>();
builder.Services.AddScoped<IEncounterParticipantRepository, EncounterParticipantRepository>();
builder.Services.AddScoped<IClinicalNoteRepository, ClinicalNoteRepository>();
builder.Services.AddScoped<IProblemRepository, ProblemRepository>();
builder.Services.AddScoped<IAllergyRepository, AllergyRepository>();
builder.Services.AddScoped<IMedicationRequestRepository, MedicationRequestRepository>();
builder.Services.AddScoped<IMedicationAdministrationRepository, MedicationAdministrationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ILabResultRepository, LabResultRepository>();
builder.Services.AddScoped<IDiagnosticReportRepository, DiagnosticReportRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IScheduleSlotRepository, ScheduleSlotRepository>();
builder.Services.AddScoped<IBillingRecordRepository, BillingRecordRepository>();
builder.Services.AddScoped<IFileReferenceRepository, FileReferenceRepository>();
builder.Services.AddScoped<IImagingReferenceRepository, ImagingReferenceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClinicianProfileRepository, ClinicianProfileRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ICodeSetRepository, CodeSetRepository>();
builder.Services.AddScoped<ICodeSetItemRepository, CodeSetItemRepository>();
builder.Services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditAccessLogRepository, AuditAccessLogRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPatientIdentifierRepository, PatientIdentifierRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add controllers & JSON options
builder.Services.AddControllers()
    .AddJsonOptions(opt => {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Authentication & Authorization placeholders (IdentityServer to be connected later)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = configuration["IdentityServer:Authority"]; // set later
        options.Audience = "ehr.api";
        options.RequireHttpsMetadata = env.IsProduction();
    });

builder.Services.AddAuthorization();

// Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EHR API", Version = "v1" });
    // JWT bearer auth in swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

// Register application/infrastructure services (implemented in extension)
builder.Services.AddApplicationServices();     // extension method - see below
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EhrDbContext>();
    //context.Database.Migrate();
    DbInitializer.Seed(context);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
