using EHR.Domain.Entities;
using EHR.Infrastructure.Persistence;
using EHR.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Infrastructure.Repositories.Implementations
{
    public class AuditAccessLogRepository : Repository<AuditAccessLog>, IAuditAccessLogRepository
    {
        public AuditAccessLogRepository(EhrDbContext context) : base(context) { }
    }
}
