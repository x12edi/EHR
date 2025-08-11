using EHR.Domain.Entities;
using EHR.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EHR.Infrastructure.SeedData
{
    public static class DbInitializer
    {
        public static void Seed(EhrDbContext context)
        {
            if (!context.Tenants.Any())
            {
                var tenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Tenant",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "system"
                };
                context.Tenants.Add(tenant);
            }

            if (!context.Departments.Any())
            {
                var dept = new Department
                {
                    Id = Guid.NewGuid(),
                    Name = "General Medicine",
                    Type = "Clinic",
                    AddressJson = "{}",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "system"
                };
                context.Departments.Add(dept);
            }

            if (!context.Locations.Any())
            {
                var deptId = context.Departments.Local.First().Id;
                var loc = new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Main Hospital Wing A",
                    Code = "L1",
                    DepartmentId = deptId,
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "system"
                };
                context.Locations.Add(loc);
            }

            if (!context.Users.Any())
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    DisplayName = "admin",
                    Email = "admin@example.com",
                    ProfileJson = "{}",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "system"
                };
                context.Users.Add(user);
            }

            if (!context.Patients.Any())
            {
                var patient = new Patient
                {
                    Id = Guid.NewGuid(),
                    MRN = "MRN1001",
                    FirstName = "John",
                    MiddleName = "P",
                    LastName = "Doe",
                    FullNameNormalized = "John Doe",
                    DOB = new DateTime(1985, 5, 15),
                    Gender = "Male",
                    AddressesJson = "{}",
                    IdentifiersJson = "{}",
                    DemographicsJson = "{}",
                    IsActive = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "system"
                };
                context.Patients.Add(patient);
            }

            context.SaveChanges();
        }
    }
}
