using System.Linq.Expressions;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IGenericRepository<T, TKey> : IEntityRepository<T, TKey> where T : class
    {
        
    }
}
