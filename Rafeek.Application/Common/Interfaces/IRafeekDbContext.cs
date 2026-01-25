using System;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IRafeekDbContext : IAsyncDisposable
    {
        DbSet<TeachingAssistant> TeachingAssistants { get; set; }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Student> Students { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
