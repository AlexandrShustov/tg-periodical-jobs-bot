using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Abstract;

namespace TelegramReminder.Model.Concrete.Commands
{
    public class ChangeTitleCommand : TelegramBotCmd
    {
        public override string Tag => "set_title";

        public ChangeTitleCommand(TelegramBot bot) : base(bot)
        { }

        public override async Task Execute(Update update, CommandArgs command)
        {
            var chatId = update?.Message?.Chat.Id ?? 0;

            var hasMessage = command.Args.TryGetValue("message", out var message);
            var hasTime = command.Args.TryGetValue("while", out var time);
            
            if(hasMessage && hasTime)
            {
                try
                {
                    var dateTime = DateTime.ParseExact(time, "dd/MM/yyyy", null);

                    var daysToWait = (int)(dateTime - DateTime.UtcNow).TotalDays;
                    var title = string.Format(message, daysToWait.ToString());

                    await Bot.Client.SetChatTitleAsync(chatId, title);
                }
                catch(Exception e)
                {
                    await Bot.Client.SendTextMessageAsync(chatId, $"Something went wrong: {e.Message}");
                }
            }
        }
    }
}
