using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Jobs
{
    public class TelegramJob : IPeriodicalJob
    {
        private TelegramBot _bot;
        private Update _update;
        private CommandArgs _commandArgs;

        public string CronIntervalExpression { get; private set; }
        public bool AutoRestart { get; private set; }
        public TimeZoneInfo TimeZone { get; private set;}
        
        public TelegramJob(Update update, TelegramBot bot, CommandArgs args)
        {
            _bot = bot;
            _update = update;
            _commandArgs = args;
        }

        public async Task Execute()
        {
            if(_bot.CanUnderstand(_commandArgs.Tag))
                await _bot.Execute(_update, _commandArgs);
        }

        internal TelegramJob WithCrone(string crone)
        {
            CronIntervalExpression = crone;
            return this;
        }

        internal TelegramJob WithAutoRestart(bool value)
        {
            AutoRestart = value;
            return this;
        }

        internal TelegramJob WithTimeZone(TimeZoneInfo timeZone)
        {
            TimeZone = timeZone;
            return this;
        }
    }
}
