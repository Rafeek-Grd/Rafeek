using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class BaseIdentityEntityRepository<T, TKey> : EntityRepository<T, TKey> where T : class
    {
        protected new readonly IRafeekIdentityDbContext _context;

        public BaseIdentityEntityRepository(IRafeekIdentityDbContext context) : base((DbContext)context)
        {
            _context = context;
        }
    }
}
