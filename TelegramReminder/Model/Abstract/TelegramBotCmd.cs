using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Abstract
{
    public abstract class TelegramBotCmd
    {
        public abstract string Tag { get; }
        public abstract IEnumerable<string> RequiredArgs { get; }

        protected readonly TelegramBot Bot;

        public TelegramBotCmd(TelegramBot bot) =>
            Bot = bot;

        public abstract Task Execute(Update update, CommandArgs args);

        public virtual bool CanBeExecuted(CommandArgs args)
        {
            var hasAllRequiredParams = RequiredArgs
                .All(a => args.Args
                    .TryGetValue(a, out var _));

            return args.Tag == Tag && hasAllRequiredParams;
        }
    }
}
