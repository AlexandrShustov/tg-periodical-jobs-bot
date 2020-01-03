using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete.Commands.Tasks
{
    public class TelegramDelayedTask : IEditDelayedTask
    {
        public long ChatId => _update?.ChatId() ?? 0;

        public string Name => "undefined";

        public string Cron { get; protected set; }
        public TimeZoneInfo TimeZone { get; protected set; } = TimeZoneInfo.Utc;
        public bool AutoRestart { get; protected set; }
        public DateTime Deadline { get; private set; }

        public bool CanBeStarted => 
            AutoRestart && Deadline >= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
        
        private Context _commandArgs;
        private Command _cmd;
        private Update _update;

        public TelegramDelayedTask(Update update, Command cmd, Context args, string cron)
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

        public TelegramDelayedTask WithDeadline(DateTime deadline)
        {
            Deadline = deadline;
            return this;
        }

        public Task Execute() =>
            Task.CompletedTask;

        public void Disable() =>
            AutoRestart = false;
    }
}
