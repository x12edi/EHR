// EHR.Domain.Entities/Tenant.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Tenant : AuditableEntity
    {
        public string Name { get; set; }
        public string DataIsolationMode { get; set; } // Schema/DB/Shared
        public string ConfigJson { get; set; }
    }
}
