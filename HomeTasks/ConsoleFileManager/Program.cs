namespace ConsoleFileManager
{
    using Microsoft.Extensions.DependencyInjection;

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
