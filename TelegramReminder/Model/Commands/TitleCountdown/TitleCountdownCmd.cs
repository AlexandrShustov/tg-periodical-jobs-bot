using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Extensions;
using TelegramReminder.Services.Abstract;

namespace TelegramReminder.Model.Commands.TitleCountdown
{
    public class TitleCountdownCmd //: IDelayedTaskConvertible
    {
        public string Tag => "title_countdown";
        public string Description => "Creates a task that will rename the channel using specified text, which will be modified by incrementing number {0}." +
            @" Usage: /title_countdown cron deadline message autorestart? timezone?";

        public async Task Execute(IBotService bot, Update update, TitleCountdownArgs args)
        {
            var message = args.Message;
            var deadline = args.Deadline;

            var daysToWait = (int)(deadline - DateTime.UtcNow).TotalDays;
            var title = string.Format(message, daysToWait.ToString());

            await bot.Client.SetChatTitleAsync(update.ChatId(), title);
        }

        //public IEditDelayedTask ToDelayedTask(Context args, Update update)
        //{
        //    var cron = args.ArgumentOrEmpty("cron");
        //    var deadline = args.ArgumentOrEmpty("deadline").ToDateTime();
        //    var autorestart = args.ArgumentOrEmpty("autorestart").ToBool();
        //    var timezone = args.ArgumentOrEmpty("timezone").ToTimeZone();

        //    var job = new TelegramDelayedTask(update, this, args, cron)
        //        .WithAutoRestart(autorestart);

        //    if (timezone != null)
        //        job.WithTimezone(timezone);

        //    if (deadline != default)
        //        job.WithDeadline(deadline);

        //    return job;
        //}
    }
}
