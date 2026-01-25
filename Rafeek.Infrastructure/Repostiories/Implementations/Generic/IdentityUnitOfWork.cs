using System.Threading;
using System.Threading.Tasks;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private readonly IRafeekIdentityDbContext _identityDbContext;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;

        private RefreshTokenRepository? _refreshTokenRepository;

        public IdentityUnitOfWork
        (
            IRafeekIdentityDbContext identityDbContext,
            IJwtTokenManager jwtTokenManager,
            ICurrentUserService currentUserService
        )
        {
            _identityDbContext = identityDbContext;
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
        }

        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_identityDbContext, _jwtTokenManager, _currentUserService);

        public async ValueTask DisposeAsync()
        {
            await _identityDbContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _identityDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
