using System;
using System.Collections.Generic;
using System.Linq;
using TelegramReminder.Model.Abstract;

namespace TelegramReminder.Model.Concrete
{
    public class PeriodicalJobs : IPeriodicalJobs
    {
        public event Action Updated;
        public bool Any => _pendingJobs.Any();

        private Queue<IPeriodicalJob> _pendingJobs = new Queue<IPeriodicalJob>();

        public IPeriodicalJob Pop()
        {
            if (!_pendingJobs.Any())
                return null;

            return _pendingJobs.Dequeue();
        }

        public void Push(IPeriodicalJob job)
        {
            _pendingJobs.Enqueue(job);
            Updated?.Invoke();
        }
    }
}
