using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Extension
{
    public static class DatabaseConfigurations
    {
        public static void ConfigureDB(this IServiceCollection services, string connectionString, string useDb)
        {
            services.AddDbContext<AutomatContext>(options =>
            {
                options.UseLazyLoadingProxies();
                if (!options.IsConfigured)
                {
                    if (useDb == "MySQL")
                    {
                        options.UseMySQL(connectionString);
                    }
                    else if (useDb == "SQLite")
                    {
                        options.UseSqlite(connectionString);
                    }
                    else
                    {
                        throw new Exception("No Database selected");
                    }
                }
            });
        }
    }
}