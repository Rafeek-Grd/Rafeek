using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;
using Rafeek.Persistence;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class InstructorRepository : BaseEntityRepository<Instructor, Guid>, IInstructorRepository
    {
        public InstructorRepository(IRafeekDbContext context) : base(context)
        {
        }
    }
}
