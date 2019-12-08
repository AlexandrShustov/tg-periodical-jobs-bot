using Cronos;
using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class CronBasedScheduler
    {
        private IDelayedTask _job;
        private bool _autoRestart;

        public CronBasedScheduler Schedule(IDelayedTask job)
        {
            _job = job;
            _autoRestart = job.AutoRestart;

            return this;
        }

        public void Start()
        {
            var timeToWait = CronExpression
                .Parse(_job.Cron)
                .GetNextOccurrence(DateTime.UtcNow, _job.TimeZone).Value;

            var now = DateTime.UtcNow;

            Task.Delay(timeToWait - now)
                .ContinueWith(t => InvokeEventAndRestartIfNeeded());
        }

        private async Task InvokeEventAndRestartIfNeeded()
        {
            await _job.Execute();

            if (!_autoRestart)
                return;

            Start();
        }
    }
}
