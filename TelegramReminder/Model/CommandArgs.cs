using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TelegramReminder.Model
{
    public class Context
    {
        public Update Update { get; }
        public string Tag { get; set; }

        public Context(string tag, Update update)
        {
            Tag = tag;
            Update = update;
        }

        public LocalContext UseLocal(object with) =>
            new LocalContext(Tag, Update, with);

        public ExternalContext UseExternal(IReadOnlyDictionary<string, string> with) =>
            new ExternalContext(Tag, Update, with);

        public LocalContext AsLocal() =>
            this as LocalContext;

        public ExternalContext AsExternal() =>
            this as ExternalContext;
    }

    public class LocalContext : Context
    {
        public object Args { get; set; }

        public LocalContext(string tag, Update update, object args) : base(tag, update) =>
            Args = args ?? new object();

        public virtual T UseArgument<T>() where T : class =>
            Args as T;
    }

    public class ExternalContext : Context
    {
        public IReadOnlyDictionary<string, string> Args { get; set; }

        public ExternalContext(string tag, Update update, IReadOnlyDictionary<string, string> args) : base(tag, update)
        {
            Args = args ?? new Dictionary<string, string>();
        }

        public virtual T UseArgument<T>() where T : class =>
            Arguments.Using<T>(Args);
    }
}
