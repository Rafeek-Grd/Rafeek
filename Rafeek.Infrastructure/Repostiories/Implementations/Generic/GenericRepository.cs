using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Linq.Expressions;
using Rafeek.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class GenericRepository<T, TKey> : EntityRepository<T, TKey>, IGenericRepository<T, TKey> where T : class
    {
        public GenericRepository(RafeekDbContext dbContext) : base(dbContext)
        {
        }
    }
}
