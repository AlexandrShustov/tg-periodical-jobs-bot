using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;
using TelegramReminder.Model.Extensions;
using TelegramReminder.Model.Jobs;

namespace TelegramReminder.Model.JobSelection.Concrete
{
    public class ChangeTitleJobSelector : JobSelector
    {
        public override string Tag => "set_title";

        public override bool SatisfiedBy(CommandArgs args) =>
            args.Has("cron") &&
            args.Has("deadline") &&
            args.Has("message");

        public override IPeriodicalJob Select(Update update, TelegramBot bot, CommandArgs args)
        {
            var cron = args.ArgumentOrEmpty("cron");
            var autorestart = args.ArgumentOrEmpty("autorestart").ToBool();
            var timezone = args.ArgumentOrEmpty("timezone").ToTimeZone();

            return new ChangeTitleJob(update, bot, args, cron)
                .WithAutoRestart(autorestart)
                .WithTimeZone(timezone);
        }
    }
}
