using System.Collections.Generic;

namespace TelegramReminder.Model.Extensions
{
    public static class CleanCodeExtensions
    {
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value)
                ? value 
                : default;
        }
    }
}
