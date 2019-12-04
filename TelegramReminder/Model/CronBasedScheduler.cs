using Cronos;
using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class CronBasedScheduler
    {
        private IPeriodicalJob _job;
        private bool _autoRestart;

        public CronBasedScheduler Schedule(IPeriodicalJob job)
        {
            _job = job;
            return this;
        }

        public CronBasedScheduler WithAutoRestart(bool value)
        {
            _autoRestart = value;
            return this;
        }

        public void Start()
        {
            var timeToWait = CronExpression
                .Parse(_job.CronIntervalExpression)
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
