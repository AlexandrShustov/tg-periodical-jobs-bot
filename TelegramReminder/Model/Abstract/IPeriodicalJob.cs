using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public interface IDelayedTask
    {
        string Cron { get; }
        TimeZoneInfo TimeZone { get; } 
        bool AutoRestart { get; }

        Task Execute();
    }
}
