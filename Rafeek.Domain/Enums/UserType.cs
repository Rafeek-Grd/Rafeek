namespace Rafeek.Domain.Enums
{
    [Flags]
    public enum UserType
    {
        None = 0,
        Admin = 1,
        SubAdmin = 2,
        Staff = 4,
        Instructor = 8,
        Doctor = 16,
        Student = 32
    }
}
