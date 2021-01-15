using Microsoft.Extensions.DependencyInjection;

namespace ConsoleFileManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceCollection()
                .AddSingleton<IManager, FileManagerAdapter>()
                .BuildServiceProvider();
            CommandHandler handler = new CommandHandler(service.GetService<IManager>());
            handler.Run();
        }
    }
}
