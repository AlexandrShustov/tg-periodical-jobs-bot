using System;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Jobs
{
    public class ChangeTitleJob : TelegramPeriodicalJob
    {
        public ChangeTitleJob(Update update, TelegramBot bot, CommandArgs args, string cron)
            : base(update, bot, args) => CronIntervalExpression = cron;

        internal ChangeTitleJob WithAutoRestart(bool value)
        {
            AutoRestart = value;
            return this;
        }

        internal ChangeTitleJob WithTimeZone(TimeZoneInfo timeZone)
        {
            TimeZone = timeZone;
            return this;
        }
    }
}
