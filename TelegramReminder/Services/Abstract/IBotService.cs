using Telegram.Bot;

namespace TelegramReminder.Services.Abstract
{
    public interface IBotService
    {
        TelegramBotClient Client { get; } 
    }
}
