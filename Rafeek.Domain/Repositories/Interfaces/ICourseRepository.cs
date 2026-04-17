using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Domain.Repositories.Interfaces
{
    public interface ICourseRepository: IGenericRepository<Course, Guid>
    {
    }
}
