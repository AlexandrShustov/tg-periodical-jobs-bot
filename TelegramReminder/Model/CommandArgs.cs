using System.Collections.Generic;
using System.Linq;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Model
{
    public class CommandArgs
    {
        public string Tag { get; set; }
        public IReadOnlyDictionary<string, string> Args { get; set; }

        public CommandArgs(string tag, IReadOnlyDictionary<string, string> args)
        {
            Tag = tag;
            Args = args;
        }

        public bool Has(string arg) => 
            !ArgumentOrEmpty(arg).IsNullOrEmpty();

        public string ArgumentOrEmpty(string key)
        {
            if (Args == null || !Args.Any())
                return string.Empty;

            var contains = Args.TryGetValue(key, out var result);

            if (contains is false)
                return string.Empty;

            return result;
        }
    }
}
