using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Abstract
{
    public abstract class TelegramBotCmd
    {
        public abstract string Tag { get; }

        protected readonly TelegramBot Bot;

        public TelegramBotCmd(TelegramBot bot) =>
            Bot = bot;

        public abstract Task Execute(Update update, CommandArgs args);
    }
}
