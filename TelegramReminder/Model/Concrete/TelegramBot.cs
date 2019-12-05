using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Concrete.Commands;

namespace TelegramReminder.Model.Concrete
{
    public class TelegramBot
    {
        public readonly TelegramBotClient Client;

        private User _user;
        private readonly List<TelegramBotCmd> _knownCommands = new List<TelegramBotCmd>();

        public TelegramBot(string token, string webhookUrl)
        {
            Client = new TelegramBotClient(token);
            Client.SetWebhookAsync(webhookUrl).Wait();
                        
            _knownCommands.Add(new ChangeTitleCommand(this));
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

            return command.Execute(update, commandArgs);
        }
    }
}
