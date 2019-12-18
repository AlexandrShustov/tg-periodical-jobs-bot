using Cronos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class Scheduler
    {
        public IEditDelayedTask DelayedTask { get; private set; }

        private Action<Scheduler> _whenFinish;
        private Action<Scheduler> _whenError;
        private Task<Task> _taskHandle;

        public Scheduler Schedule(IEditDelayedTask task)
        {
            DelayedTask = task;
            return this;
        }

        public void Start()
        {
            var cancelTokenSource = new CancellationTokenSource();

            try
            {
                var timeToWait = CronExpression
                    .Parse(DelayedTask.Cron)
                    .GetNextOccurrence(DateTime.UtcNow, DelayedTask.TimeZone).Value;

                var now = DateTime.UtcNow;

                _taskHandle = Task.Delay(timeToWait - now, cancelTokenSource.Token)
                    .ContinueWith(t => InvokeEventAndRestartIfNeeded());
            }
            catch(Exception e)
            {
                _whenError?.Invoke(this);

                if (!_taskHandle.IsCompleted)
                    cancelTokenSource.Cancel();

                _taskHandle = null;
            }
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

        public Scheduler WhenFinished(Action<Scheduler> then)
        {
            _whenFinish = then;
            return this;
        }

        internal Scheduler WhenError(Action<Scheduler> whenError)
        {
            _whenError = whenError;
            return this;
        }
    }
}
