using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTelegramBot(this IServiceCollection services, IConfiguration config)
        {
            var appKey = config["Key"];
            var url = config["Url"];

            var bot = new TelegramBot(appKey, url);
            services.AddSingleton(bot);
        }
    }
}
