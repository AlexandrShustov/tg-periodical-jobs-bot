using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete
{
    public class StopTaskCmd : Command
    {
        public override string Tag => "stop_task";
        public override IEnumerable<string> RequiredArgs => new[] { "id" };

        public StopTaskCmd(TelegramBot bot) : base(bot)
        { }

        public async override Task Execute(Update update, CommandArgs args)
        {
            var id = args.ArgumentOrEmpty("id").ToInt();

            if (id == null)
                throw new ArgumentException("Argument id coudn`t be parsed");

            if (Bot.Tasks.Count() < id)
                throw new ArgumentException("There is no running task with specified id");

            Bot.StopTask(id.Value);

            await $"Task {id.Value} was stopped"
                .AsMessageTo(update.ChatId(), Bot.Client);
        }
    }
}
