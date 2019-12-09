using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramReminder.Model.Extensions
{
    public static class StringExtensions
    {
        public async static Task IfNullOrEmpty(this string self, Task then)
        {
            if (self == null || self == string.Empty)
                await then;
        }

        public static Task<Message> AsMessageTo(this string self, long chatId, TelegramBotClient client) =>
            client.SendTextMessageAsync(chatId, self);

        public static bool ToBool(this string self)
        {
            var res = bool.TryParse(self, out var value);

            return res
                ? value
                : default;
        }

        public static TimeZoneInfo ToTimeZone(this string self)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(self);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static DateTime ToDateTime(this string self)
        {
            try
            {
               return DateTime.ParseExact(self, "dd/MM/yyyy", null);
            }
            catch(Exception e)
            {
                return default;
            }
        }

        public static bool IsNullOrEmpty(this string self) =>
            self == null || self == string.Empty;
    }
}
