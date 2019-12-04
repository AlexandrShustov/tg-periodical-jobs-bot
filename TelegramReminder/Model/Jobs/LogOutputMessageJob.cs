using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TelegramReminder.Model.Jobs
{
    public class LogOutputMessageJob : IPeriodicalJob
    {
        public string CronIntervalExpression => "* * * * *";
        public bool AutoRestart => true;

        public TimeZoneInfo TimeZone { get; private set; }

        public Task Execute()
        {
            Debug.WriteLine("===============Debug job working===============");
            return Task.CompletedTask;
        }

        public LogOutputMessageJob WithZone(TimeZoneInfo zone)
        {
            TimeZone = zone;
            return this;
        }
    }
}
