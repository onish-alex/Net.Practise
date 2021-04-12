using DI.App.Services;
using DI.App.Services.PL;
using DI.App.Services.PL.Commands;
using DI.App.Abstractions;
using DI.App.Abstractions.BLL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DI.App
{
    internal class Program
    {
        private static void Main()
        {
            var service = new ServiceCollection();
            service.AddSingleton<ICommandProcessor, CommandProcessor>();
            service.AddSingleton<IUserStore, UserStore>();
            service.AddSingleton<IDatabaseService, InMemoryDatabaseService>();
            service.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Transient<ICommand, AddUserCommand>(),
                ServiceDescriptor.Transient<ICommand, ListUsersCommand>(),
            });
            service.AddTransient<AddUserCommand>();
            service.AddTransient<ListUsersCommand>();
            
            var provider = service.BuildServiceProvider(new ServiceProviderOptions());
            
            var manager = new CommandManager(provider.GetService<ICommandProcessor>());
            manager.Start();

        }
    }
}
