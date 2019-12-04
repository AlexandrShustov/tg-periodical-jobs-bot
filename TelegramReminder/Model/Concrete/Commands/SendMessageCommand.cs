using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;

namespace TelegramReminder.Model.Concrete.Commands
{
    public class SendMessageCommand
    {
        public Update Update => _update;
        public string Message { get; }

        private readonly Update _update;

        public SendMessageCommand(Update update, string text)
        {
            _update = update;
            Message = text;
        }
    }
}
