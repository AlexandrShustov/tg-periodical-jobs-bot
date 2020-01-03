using System;
using Telegram.Bot.Types;

namespace TelegramReminder.Model.Abstract
{
    public interface IDelayedTaskConvertible
    {
        IEditDelayedTask ToDelayedTask(Context args, Update update);
    }
}
