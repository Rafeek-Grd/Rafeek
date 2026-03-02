using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class BaseEntityRepository<T, TKey> : EntityRepository<T, TKey> where T : class
    {
        protected readonly IRafeekDbContext? _context;
        protected readonly IRafeekIdentityDbContext? _identityContext;

        public BaseEntityRepository(IRafeekDbContext context) : base((DbContext)context)
        {
            _context = context;
            _identityContext = null;
        }

        public BaseEntityRepository(IRafeekIdentityDbContext identityContext) : base((DbContext)identityContext)
        {
            _identityContext = identityContext;
            _context = null;
        }
    }
}
