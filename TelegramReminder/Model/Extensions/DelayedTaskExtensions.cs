using System.Collections.Generic;
using System.Linq;

namespace TelegramReminder.Model.Extensions
{
    public static class DelayedTaskExtensions
    {
        public static IEnumerable<IDelayedTask> Of(this IEnumerable<IDelayedTask> self, long? chatId) =>
            chatId.HasValue
            ? self.Where(t => t.ChatId == chatId.Value)
            : Enumerable.Empty<IDelayedTask>();
    }
}
