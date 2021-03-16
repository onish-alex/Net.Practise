using App.DataAccess.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace App.DataAccess.Settings
{
    public static class DependencySettings
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
