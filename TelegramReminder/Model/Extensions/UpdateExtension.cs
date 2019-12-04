using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramReminder.Model.Extensions
{
    public static class UpdateExtension
    {
        public static long ChatId(this Update self) =>
            self.Message.Chat.Id;
    }
}
