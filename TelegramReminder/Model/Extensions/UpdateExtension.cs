using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramReminder.Model.Extensions
{
    public static class UpdateExtension
    {
        public static long ChatId(this Update self) =>
            self.Message.Chat.Id;

        public static bool HasMentionOf(this Update self,  User user)
        {
            var hasMentions = self.Message?
                .Entities
                .Any(e => e.Type == MessageEntityType.Mention);
                        
            return self.Message.Text.Contains($"@{user.Username}");
        }
    }
}
