using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.JobSelection
{
    public abstract class JobSelector
    {
        public abstract string Tag { get; }
        public abstract bool SatisfiedBy(CommandArgs args);
        public abstract IPeriodicalJob Select(Update update, TelegramBot bot, CommandArgs args);
    }
}
