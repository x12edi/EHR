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
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(EhrDbContext context) : base(context) { }
    }
}
