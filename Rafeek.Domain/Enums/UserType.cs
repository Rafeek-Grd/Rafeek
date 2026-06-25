namespace Rafeek.Domain.Enums
{
    [Flags]
    public enum UserType
    {
        None = 0,
        Admin = 1,
        Staff = 2,
        Mentor = 4,
        Professor = 8,
        Student = 16
    }
}
