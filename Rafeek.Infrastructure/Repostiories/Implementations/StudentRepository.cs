using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class StudentRepository: BaseEntityRepository<Student, Guid>, IStudentRepository
    {
        public StudentRepository(IRafeekDbContext context): base(context)
        {
            
        }
    }
}
