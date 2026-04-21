using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class ReminderRepository : BaseEntityRepository<Reminder, Guid>, IReminderRepository
    {
        public ReminderRepository(IRafeekDbContext context) : base(context)
        {
        }
    }
}
