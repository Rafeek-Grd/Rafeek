using MediatR;
using Rafeek.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Rafeek.Application.Handlers.AuthHandlers.SignUp
{
    public class SignUpCommand : IRequest<SignResponse>
    {
        public UserType PrimaryRole { get; set; } = UserType.Student;
        public List<UserType>? AdditionalRoles { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ImageName { get; set; }
        public string Password { get; set; } = null!;
        public string PasswordConfirm { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public GenderType? Gender { get; set; }
        public string? BirthDate { get; set; }
        public string FbToken { get; set; } = null!;
        public bool IsAndroidDevice { get; set; }
        public bool IsIosDevice { get; set; }
    }
}
