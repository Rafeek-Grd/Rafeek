using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Infrastructure.Oauth;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRafeekDbContext _context;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDataEncryption _dataEncryption;


        private RefreshTokenRepository? _refreshTokenRepository;
        private UserFbTokenRepository? _userFbTokenRepository;

        public UnitOfWork(
            IRafeekDbContext context,
            IJwtTokenManager jwtTokenManager,
            ICurrentUserService currentUserService,
            IDataEncryption dataEncryption)
        {
            _context = context;
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
            _dataEncryption = dataEncryption;

        }


        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_context, _jwtTokenManager, _currentUserService,_dataEncryption);
        public IUserFbTokenRepository UserFbTokenRepository => _userFbTokenRepository ??= new UserFbTokenRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BeginTransactionAsync(cancellationToken);
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.CreateExecutionStrategy();
        }


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
