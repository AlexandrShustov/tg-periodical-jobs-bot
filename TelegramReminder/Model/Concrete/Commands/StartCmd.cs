using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete.Commands
{
    public class StartCmd : Command
    {
        public override string Tag => "start";

        public StartCmd(TelegramBot bot) : base(bot)
        { }

        public async override Task Execute(Update update, CommandArgs args)
        {
            var commandsInfo = string.Join("\n", Bot.Commands
                .Where(c => c != this)
                .Select(c => $"{c.Tag} : {c.Description}"));

            var text = $"Hello! This what you can do: \n{commandsInfo}";

            await Bot.Client.SendTextMessageAsync(update.ChatId(), text);
        }
    }
}
