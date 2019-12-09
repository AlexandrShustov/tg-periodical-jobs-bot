using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete.Commands.Tasks
{
    public class TitleCountdownCmd : Command, IDelayed
    {
        public override string Tag => "set_title";
        public override IEnumerable<string> RequiredArgs { get; } =
            new List<string> { "cron", "deadline", "message", };

        public TitleCountdownCmd(TelegramBot bot) : base(bot)
        { }

        public override async Task Execute(Update update, CommandArgs args)
        {
            var message = args.ArgumentOrEmpty("message");
            var deadline = args.ArgumentOrEmpty("deadline");

            if (message.IsNullOrEmpty() || deadline.IsNullOrEmpty())
                return;

            try
            {
                var dateTime = deadline.ToDateTime();

                var daysToWait = (int)(dateTime - DateTime.UtcNow).TotalDays;
                var title = string.Format(message, daysToWait.ToString());

                await Bot.Client.SetChatTitleAsync(update.ChatId(), title);
            }
            catch (Exception e)
            {
                await Bot.Client.SendTextMessageAsync(update.ChatId(), $"Something went wrong: {e.Message}");
            }
        }

        public IDelayedTask ToDelayedTask(CommandArgs args, Update update)
        {
            var cron = args.ArgumentOrEmpty("cron");
            var deadline = args.ArgumentOrEmpty("deadline").ToDateTime();
            var autorestart = args.ArgumentOrEmpty("autorestart").ToBool();
            var timezone = args.ArgumentOrEmpty("timezone").ToTimeZone();

            var job = new TelegramDelayedTask(update, this, args, cron)
                .WithAutoRestart(autorestart)
                .WithTimezone(timezone)
                .WithDeadline(deadline);

            return job;
        }
    }
}
