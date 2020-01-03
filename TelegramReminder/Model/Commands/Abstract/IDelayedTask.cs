using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public interface IDelayedTask
    {
        long ChatId { get; }

        string Name { get; }

        string Cron { get; }
        TimeZoneInfo TimeZone { get; } 
        DateTime Deadline { get; }
        bool CanBeStarted { get; }
        bool AutoRestart { get; }

        Task Execute();
    }

    public interface IEditDelayedTask : IDelayedTask
    {
        void Disable();
    }
}
