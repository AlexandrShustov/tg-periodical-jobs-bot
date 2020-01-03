using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramReminder.Services;
using TelegramReminder.Services.Abstract;

namespace TelegramReminder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTelegramBot(this IServiceCollection services, IConfiguration config)
        {
            var appKey = config["Key"];
            var url = config["Url"];

            var bot = new BotService(appKey, url);
            services.AddSingleton<IBotService>(bot);
        }
    }
}
