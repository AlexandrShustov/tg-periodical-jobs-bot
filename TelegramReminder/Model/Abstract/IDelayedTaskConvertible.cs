using System;
using Telegram.Bot.Types;

namespace TelegramReminder.Model.Abstract
{
    public interface IDelayedTaskConvertible
    {
        IEditDelayedTask ToDelayedTask(CommandArgs args, Update update);
    }
}
