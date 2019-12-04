using System.Collections.Generic;

namespace TelegramReminder.Model
{
    public class CommandArgs
    {
        public string Tag { get; set; }
        public IReadOnlyDictionary<string, string> Args { get; set; }
    }
}
