using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Persistence;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RafeekDbContext _rafeekDbContext;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;

        private RefreshTokenRepository? _refreshTokenRepository;

        public UnitOfWork
        (
            RafeekDbContext rafeekDbContext,
            IJwtTokenManager jwtTokenManager,
            ICurrentUserService currentUserService
        )
        {
            _rafeekDbContext = rafeekDbContext;
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
        }

        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_rafeekDbContext, _jwtTokenManager, _currentUserService);

        public async ValueTask DisposeAsync()
        {
            await _rafeekDbContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _rafeekDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
