using System;

namespace TelegramReminder.Model.Abstract
{
    public interface IPeriodicalJobs
    {
        event Action Updated;

        bool Any { get; }

        IPeriodicalJob Pop();
        void Push(IPeriodicalJob job);
    }
}
