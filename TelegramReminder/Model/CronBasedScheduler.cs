using Cronos;
using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class CronBasedScheduler
    {
        public IDelayedTask DelayedTask { get; private set; }

        private bool _autoRestart;

        public CronBasedScheduler Schedule(IDelayedTask task)
        {
            DelayedTask = task;
            _autoRestart = task.AutoRestart;

            return this;
        }

        public void Start()
        {
            var timeToWait = CronExpression
                .Parse(DelayedTask.Cron)
                .GetNextOccurrence(DateTime.UtcNow, DelayedTask.TimeZone).Value;

            var now = DateTime.UtcNow;

            Task.Delay(timeToWait - now)
                .ContinueWith(t => InvokeEventAndRestartIfNeeded());
        }

        private async Task InvokeEventAndRestartIfNeeded()
        {
            await DelayedTask.Execute();

            if (!_autoRestart)
                return;

            Start();
        }
    }
}
