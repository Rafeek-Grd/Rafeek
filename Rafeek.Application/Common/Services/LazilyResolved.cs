using Microsoft.Extensions.DependencyInjection;

namespace Rafeek.Application.Common.Services
{
    public class LazilyResolved<T> : Lazy<T>
    {
        public LazilyResolved(IServiceProvider serviceProvider)
        : base(serviceProvider.GetRequiredService<T>)
        {
        }
    }
}
