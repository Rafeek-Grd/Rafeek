using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class StudentAcademicProfileRepository: BaseEntityRepository<StudentAcademicProfile, Guid>, IStudentAcademicProfileRepository
    {
        public StudentAcademicProfileRepository(IRafeekDbContext context): base(context)
        {
            
        }
    }
}
