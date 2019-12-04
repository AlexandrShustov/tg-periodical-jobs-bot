using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramReminder.Model.Concrete.Commands
{
    public class DebugMessageJob : IPeriodicalJob
    {
        public string CronIntervalExpression => "* * * * *";

        public bool AutoRestart => false;

        public TimeZoneInfo TimeZone => null;

        public Task Execute()
        {
            System.Diagnostics.Debug.WriteLine("AAAAAAAAAAAAAA");
            return Task.CompletedTask;
        }
    }
}
