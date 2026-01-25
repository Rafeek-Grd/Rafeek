using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class BaseEntityRepository<T, TKey> : EntityRepository<T, TKey> where T : class
    {
        protected new readonly IRafeekDbContext _context;

        public BaseEntityRepository(IRafeekDbContext context) : base((DbContext)context)
        {
            _context = context;
        }
    }
}
