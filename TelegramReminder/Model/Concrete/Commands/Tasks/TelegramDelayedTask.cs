using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;

namespace TelegramReminder.Model.Concrete.Commands.Tasks
{
    public class TelegramDelayedTask : IDelayedTask
    {
        public string Name => _cmd?.Tag ?? "undefined";

        public string Cron { get; protected set; }
        public TimeZoneInfo TimeZone { get; protected set; }
        public bool AutoRestart { get; protected set; }

        private CommandArgs _commandArgs;
        private Command _cmd;
        private Update _update;

        public TelegramDelayedTask(Update update, Command cmd, CommandArgs args, string cron)
        {
            _update = update;
            _commandArgs = args;
            _cmd = cmd;

            Cron = cron;
        }

        public TelegramDelayedTask WithAutoRestart(bool autorestart)
        {
            AutoRestart = autorestart;
            return this;
        }

        public TelegramDelayedTask WithTimezone(TimeZoneInfo timeZone)
        {
            TimeZone = timeZone;
            return this;
        }

        public async Task Execute() =>
            await _cmd.Execute(_update, _commandArgs);
    }
}
