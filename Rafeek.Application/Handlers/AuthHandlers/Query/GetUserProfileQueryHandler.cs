using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.AuthHandlers.Query
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse?>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRafeekDbContext _dbContext;

        public GetUserProfileQueryHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IMapper mapper, IRafeekDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetUserProfileQueryResponse?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId == Guid.Empty)
                return null;

            var user = await _userRepository.FindByKeyAsync(_currentUserService.UserId, cancellationToken);
            if (user == null) return null;

            var dto = _mapper.Map<GetUserProfileQueryResponse>(user);

            var code = await _dbContext.Students.AsNoTracking()
                .Where(s => s.UserId == user.Id)
                .Select(s => new { Priority = 1, Code = s.UniversityCode })
                .Concat(_dbContext.Staffs.AsNoTracking().Where(s => s.UserId == user.Id).Select(s => new { Priority = 2, Code = s.EmployeeCode ?? string.Empty }))
                .Concat(_dbContext.Instructors.AsNoTracking().Where(i => i.UserId == user.Id).Select(i => new { Priority = 3, Code = i.EmployeeCode ?? string.Empty }))
                .Concat(_dbContext.Doctors.AsNoTracking().Where(d => d.UserId == user.Id).Select(d => new { Priority = 4, Code = d.EmployeeCode ?? string.Empty }))
                .OrderBy(x => x.Priority)
                .Select(x => x.Code)
                .FirstOrDefaultAsync(cancellationToken);

            dto.Code = code ?? string.Empty;
            return dto;
        }
    }
}
