using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public interface IDelayedTask
    {
        string Name { get; }

        string Cron { get; }
        TimeZoneInfo TimeZone { get; } 
        bool AutoRestart { get; }

        Task Execute();
    }
}
