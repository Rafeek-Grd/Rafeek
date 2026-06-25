using MediatR;
using Rafeek.Application.Handlers.SettingsHandlers.DTOs;

namespace Rafeek.Application.Handlers.SettingsHandlers.Queries.GetAcademicSettings
{
    public class GetAcademicSettingsQuery : IRequest<AcademicSettingsDto>
    {
    }
}
