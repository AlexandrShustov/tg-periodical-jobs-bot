using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Jobs
{
    public abstract class TelegramPeriodicalJob : IPeriodicalJob
    {
        public string CronIntervalExpression { get; protected set; }
        public TimeZoneInfo TimeZone { get; protected set; }
        public bool AutoRestart { get; protected set; }

        private CommandArgs _commandArgs;
        private TelegramBot _bot;
        private Update _update;

        protected TelegramPeriodicalJob(Update update, TelegramBot bot, CommandArgs args)
        {
            _bot = bot;
            _update = update;
            _commandArgs = args;
        }

        public async Task Execute() =>
            await _bot.Execute(_update, _commandArgs);
    }
}
