using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.SettingsHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.SettingsHandlers.Queries.GetAcademicSettings
{
    public class GetAcademicSettingsQueryHandler : IRequestHandler<GetAcademicSettingsQuery, AcademicSettingsDto>
    {
        private readonly IUnitOfWork _ctx;

        public GetAcademicSettingsQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<AcademicSettingsDto> Handle(GetAcademicSettingsQuery request, CancellationToken cancellationToken)
        {
            var setting = await _ctx.AcademicSettingRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var scales = await _ctx.GradeScaleRepository.GetAll()
                .AsNoTracking()
                .OrderByDescending(s => s.GpaPoints)
                .Select(s => new GradeScaleDto
                {
                    Id = s.Id,
                    GradeLetter = s.GradeLetter,
                    MinPercentage = s.MinPercentage,
                    GpaPoints = s.GpaPoints,
                    ArabicDescription = s.ArabicDescription
                })
                .ToListAsync(cancellationToken);

            if (setting == null)
            {
                return new AcademicSettingsDto
                {
                    MaxHoursPerSemester = 18,
                    CourseCreditHours = 3,
                    AllowOverload = true,
                    IncludeTransferHours = true,
                    GradeScales = scales
                };
            }

            return new AcademicSettingsDto
            {
                MaxHoursPerSemester = setting.MaxHoursPerSemester,
                CourseCreditHours = setting.CourseCreditHours,
                AllowOverload = setting.AllowOverload,
                IncludeTransferHours = setting.IncludeTransferHours,
                GradeScales = scales
            };
        }
    }
}
