using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public interface IPeriodicalJob
    {
        string CronIntervalExpression { get; }
        bool AutoRestart { get; }
        TimeZoneInfo TimeZone { get; } 

        Task Execute();
    }
}
