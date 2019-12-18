using System;
using Telegram.Bot.Types;

namespace TelegramReminder.Model.Exceptions
{
    public class CommandLogicException : Exception
    {
        public Update Update { get; }
        public override string Message => _message;

        private string _message;

        public CommandLogicException(Update update, string message)
        {
            Update = update;
            _message = message;
        }
    }

    public static partial class CleanCodeExtensions
    {
        public static CommandLogicException AsCommandLogicException(this string self, Update update) =>
            new CommandLogicException(update, self);
    }
}
