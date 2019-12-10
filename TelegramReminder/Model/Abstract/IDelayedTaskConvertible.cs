using Telegram.Bot.Types;

namespace TelegramReminder.Model.Abstract
{
    public interface IDelayedTaskConvertible
    {
        IDelayedTask ToDelayedTask(CommandArgs args, Update update);
    }
}
