using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public interface IPeriodicalJob
    {
        string CronIntervalExpression { get; }
        TimeZoneInfo TimeZone { get; } 
        bool AutoRestart { get; }

        Task Execute();
    }
}
