using Microsoft.Extensions.DependencyInjection;

namespace TelephoneDirectory.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);

    }
}
