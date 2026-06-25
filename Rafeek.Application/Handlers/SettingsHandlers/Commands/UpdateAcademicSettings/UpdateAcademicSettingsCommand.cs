using MediatR;
using Rafeek.Application.Handlers.SettingsHandlers.DTOs;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.SettingsHandlers.Commands.UpdateAcademicSettings
{
    public class UpdateAcademicSettingsCommand : IRequest<bool>
    {
        public int MaxHoursPerSemester { get; set; }
        public int CourseCreditHours { get; set; }
        public bool AllowOverload { get; set; }
        public bool IncludeTransferHours { get; set; }
        public List<GradeScaleDto> GradeScales { get; set; } = new();
    }
}
