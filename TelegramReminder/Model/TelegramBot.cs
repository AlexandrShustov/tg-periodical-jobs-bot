using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Concrete.Commands;
using TelegramReminder.Model.Concrete.Commands.Tasks;

namespace TelegramReminder.Model.Concrete
{
    public class TelegramBot
    {
        public readonly TelegramBotClient Client;

        private List<Scheduler> _runningTasks;
        public IEnumerable<IDelayedTask> Tasks => _runningTasks.Select(s => s.DelayedTask);

        private User _user;
        private readonly List<Command> _knownCommands = new List<Command>();

        public TelegramBot(string token, string webhookUrl)
        {
            Client = new TelegramBotClient(token);
            Client.SetWebhookAsync(webhookUrl).Wait();

            _knownCommands.AddRange(new Command[]
            {
                new TitleCountdownCmd(this),
                new GetRunningTasksCmd(this)
            });

            _runningTasks = new List<Scheduler>();
        }

        public async Task<User> User() =>
            _user ?? (_user = await Client.GetMeAsync());

        public bool CanExecute(CommandArgs args) =>
            _knownCommands.Any(c => c.CanBeExecuted(args));

        internal Task Execute(Update update, CommandArgs commandArgs)
        {
            var tag = commandArgs.Tag;
            var command = _knownCommands.FirstOrDefault(c => c.Tag == tag);

            if (command == null)
                return Task.CompletedTask;

            if(command is IDelayedTaskConvertible)
            {
                var delayedCommand = command as IDelayedTaskConvertible;
                var job = delayedCommand.ToDelayedTask(commandArgs, update);

                var scheduler = new Scheduler()
                    .Schedule(job)
                    .WhenFinished(Remove);

                scheduler.Start();

                _runningTasks.Add(scheduler);
                return Task.CompletedTask;
            }

            return command.Execute(update, commandArgs);
        }

        private void Remove(Scheduler scheduler) =>
            _runningTasks.Remove(scheduler);
    }
}
