using MediatR;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public sealed record GetProfilesForAdminsQuery(
        Guid UserId,
        List<UserType>? UserTypes
    ) : IRequest<GetProfilesForAdminsResponse>;
}
