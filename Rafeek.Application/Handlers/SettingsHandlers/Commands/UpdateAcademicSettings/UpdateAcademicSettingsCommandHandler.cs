using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.SettingsHandlers.Commands.UpdateAcademicSettings
{
    public class UpdateAcademicSettingsCommandHandler : IRequestHandler<UpdateAcademicSettingsCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public UpdateAcademicSettingsCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Handle(UpdateAcademicSettingsCommand request, CancellationToken cancellationToken)
        {
            var setting = await _ctx.AcademicSettingRepository.GetAll().FirstOrDefaultAsync(cancellationToken);
            if (setting == null)
            {
                setting = new AcademicSetting { Id = Guid.NewGuid() };
                _ctx.AcademicSettingRepository.Add(setting);
            }

            setting.MaxHoursPerSemester = request.MaxHoursPerSemester;
            setting.CourseCreditHours = request.CourseCreditHours;
            setting.AllowOverload = request.AllowOverload;
            setting.IncludeTransferHours = request.IncludeTransferHours;

            // Retrieve and clear existing grade scales
            var existingScales = await _ctx.GradeScaleRepository.GetAll().ToListAsync(cancellationToken);
            if (existingScales.Count > 0)
            {
                _ctx.GradeScaleRepository.DeleteRange(existingScales);
            }

            // Insert new grade scales
            foreach (var scaleDto in request.GradeScales)
            {
                var newScale = new GradeScale
                {
                    Id = Guid.NewGuid(),
                    GradeLetter = scaleDto.GradeLetter,
                    MinPercentage = scaleDto.MinPercentage,
                    GpaPoints = scaleDto.GpaPoints,
                    ArabicDescription = scaleDto.ArabicDescription
                };
                _ctx.GradeScaleRepository.Add(newScale);
            }

            await _ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
