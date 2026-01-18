namespace Rafeek.Application.Common.Exceptions
{
    public class UnauthorizedException : UnauthorizedAccessException
    {
        public UnauthorizedException()
            : base()
        {
        }

        public UnauthorizedException(string message) :
            base(message)
        {
            Console.WriteLine("dd");
        }
    }
}
