using EHR.API.Extensions;
using EHR.Infrastructure.Persistence;
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
