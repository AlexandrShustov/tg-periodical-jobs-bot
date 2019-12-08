using Telegram.Bot.Types;

namespace TelegramReminder.Model.Abstract
{
    public interface IDelayed
    {
        IDelayedTask ToDelayedTask(CommandArgs args, Update update);
    }
}
