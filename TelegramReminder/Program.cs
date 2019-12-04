using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TelegramReminder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(AddJsonFile)
                .UseStartup<Startup>();

        private static void AddJsonFile(WebHostBuilderContext builder, IConfigurationBuilder configBuilder)
        {
            configBuilder.Sources.Clear();
            configBuilder.AddJsonFile("config.json", false, true);
        }
    }
}
