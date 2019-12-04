using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelegramReminder.Model;
using TelegramReminder.Model.Abstract;

namespace TelegramReminder.Services
{
    public class PeriodicalJobsService : IHostedService
    {
        private readonly List<CronBasedScheduler> _schedulers;
        private readonly IPeriodicalJobs _jobs;
        
        public PeriodicalJobsService(IPeriodicalJobs jobs)
        {
            _jobs = jobs;
            _schedulers = new List<CronBasedScheduler>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _jobs.Updated += ScheduleJob;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _jobs.Updated -= ScheduleJob;
            return Task.CompletedTask;
        }

        private void ScheduleJob()
        {
            var job = _jobs.Pop();
            var scheduler = new CronBasedScheduler()
                  .Schedule(job)
                  .WithAutoRestart(job.AutoRestart);

            _schedulers.Add(scheduler);
            scheduler.Start();
        }
    }
}
