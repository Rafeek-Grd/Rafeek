using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class GradeScaleRepository : BaseEntityRepository<GradeScale, Guid>, IGradeScaleRepository
    {
        public GradeScaleRepository(IRafeekDbContext context) : base(context)
        {
        }
    }
}
