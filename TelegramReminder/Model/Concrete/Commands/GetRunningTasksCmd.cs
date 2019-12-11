using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete.Commands
{
    public class GetRunningTasksCmd : Command
    {
        public override string Tag => "get_tasks";
        public override string Description => "Returns currently running tasks for this chanell. Usage: /get_tasks";

        public GetRunningTasksCmd(TelegramBot bot) : base(bot)
        { }

        public override async Task Execute(Update update, CommandArgs args)
        {
            var message = string.Join(", ", Bot.Tasks.Of(update.ChatId()));

            await (message.IsNullOrEmpty()
                ? "no tasks found"
                : message)
                .AsMessageTo(update.ChatId(), Bot.Client);
        }
    }
}
