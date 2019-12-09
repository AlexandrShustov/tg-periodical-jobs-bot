using Cronos;
using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class Scheduler
    {
        public IDelayedTask DelayedTask { get; private set; }

        private bool _autoRestart;
        private Action<Scheduler> _whenFinish;

        public Scheduler Schedule(IDelayedTask task)
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

        public Scheduler WhenFinished(Action<Scheduler> then)
        {
            _whenFinish = then;
            return this;
        }

        private async Task InvokeEventAndRestartIfNeeded()
        {
            await DelayedTask.Execute();

            if (DelayedTask.CanBeStarted is false)
            {
                _whenFinish?.Invoke(this);
                return;
            }

            Start();
        }
    }
}
