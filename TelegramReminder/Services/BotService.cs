using Telegram.Bot;
using TelegramReminder.Services.Abstract;

namespace TelegramReminder.Services
{
    public class BotService : IBotService
    {
        public TelegramBotClient Client { get; }

        public BotService(string token, string webhookUrl)
        {
            Client = new TelegramBotClient(token);
            Client.SetWebhookAsync(webhookUrl).Wait();
        }
    }
}
