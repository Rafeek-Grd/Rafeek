namespace Rafeek.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }
        bool IsAuthenticated { get; }
        public string IpAddress { get; }
    }
}
