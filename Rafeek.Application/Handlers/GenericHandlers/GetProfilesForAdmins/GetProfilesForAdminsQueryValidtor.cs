using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public class GetProfilesForAdminsQueryValidator : AbstractValidator<GetProfilesForAdminsQuery>
    {
        private readonly IIdentityUnitOfWork _identityUnitOfWork;

        public GetProfilesForAdminsQueryValidator(IIdentityUnitOfWork identityUnitOfWork)
        {
            _identityUnitOfWork = identityUnitOfWork;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("معرف المستخدم مطلوب.")
                .MustAsync(UserExists).WithMessage("المستخدم المطلوب غير موجود.");

            When(x => x.UserTypes != null && x.UserTypes.Count > 0, () =>
            {
                RuleFor(x => x.UserTypes)
                    .Must(AllValidEnumValues).WithMessage("واحد أو أكثر من أنواع المستخدمين المحددة غير صالحة.")
                    .MustAsync(UserHasAllRequestedTypes).WithMessage("المستخدم لا يمتلك نوع المستخدم المطلوب.");
            });
        }

        private async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken)
        {
            return await _identityUnitOfWork.ApplicationUserRepository
                .ExistsAsync(u => u.Id == userId, cancellationToken);
        }

        private bool AllValidEnumValues(List<UserType> types)
        {
            return types.All(t => Enum.IsDefined(typeof(UserType), t) && t != UserType.None);
        }

        private async Task<bool> UserHasAllRequestedTypes(GetProfilesForAdminsQuery query, List<UserType> types, CancellationToken cancellationToken)
        {
            var userTypes = await _identityUnitOfWork.ApplicationUserRepository
                .GetAll()
                .AsNoTracking()
                .Where(u => u.Id == query.UserId)
                .Select(u => u.UserTypes)
                .FirstOrDefaultAsync(cancellationToken);

            return types.All(t => userTypes.HasFlag(t));
        }
    }
}
