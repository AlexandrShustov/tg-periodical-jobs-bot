using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model.Concrete.Commands.Tasks
{
    public class TitleCountdownCmd : Command, IDelayedTaskConvertible
    {
        public override string Tag => "title_countdown";
        public override IEnumerable<string> RequiredArgs { get; } = new List<string> { "cron", "deadline", "message", };
        public override string Description => "Creates a task that will rename the channel using specified text, which will be modified by incrementing number {0}." +
            @" Usage: /title_countdown cron deadline message autorestart? timezone?";

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
                await $"Something went wrong: {e.Message}"
                    .AsMessageTo(update.ChatId(), Bot.Client);
            }
        }

        public IEditDelayedTask ToDelayedTask(CommandArgs args, Update update)
        {
            var cron = args.ArgumentOrEmpty("cron");
            var deadline = args.ArgumentOrEmpty("deadline").ToDateTime();
            var autorestart = args.ArgumentOrEmpty("autorestart").ToBool();
            var timezone = args.ArgumentOrEmpty("timezone").ToTimeZone();

            var job = new TelegramDelayedTask(update, this, args, cron)
                .WithAutoRestart(autorestart);

            if (timezone != null)
                job.WithTimezone(timezone);

            if(deadline != default)
                job.WithDeadline(deadline);

            return job;
        }
    }
}
