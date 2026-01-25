using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRafeekDbContext _context;

        public UnitOfWork(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
