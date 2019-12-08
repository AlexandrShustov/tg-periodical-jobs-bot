﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramReminder.Model.Concrete;

namespace TelegramReminder.Model.Abstract
{
    public abstract class Command
    {
        public abstract string Tag { get; }
        public virtual IEnumerable<string> RequiredArgs { get; } = Enumerable.Empty<string>();

        protected readonly TelegramBot Bot;

        public Command(TelegramBot bot) =>
            Bot = bot;

        public virtual bool CanBeExecuted(CommandArgs cmdArgs)
        {
            var hasAllRequiredParams = RequiredArgs
                .All(a => cmdArgs.Args
                    .TryGetValue(a, out var _));

            return cmdArgs.Tag == Tag && hasAllRequiredParams;
        }

        public abstract Task Execute(Update update, CommandArgs args);
    }
}