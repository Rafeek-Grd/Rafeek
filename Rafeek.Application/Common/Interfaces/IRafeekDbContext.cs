using System;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IRafeekDbContext : IAsyncDisposable
    {
        DbSet<Department> Departments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
