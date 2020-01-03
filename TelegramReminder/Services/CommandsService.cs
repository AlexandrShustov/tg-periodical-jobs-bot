using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramReminder.Model;
using TelegramReminder.Model.Commands.Debug;
using TelegramReminder.Model.Commands.TitleCountdown;
using TelegramReminder.Model.Extensions;
using TelegramReminder.Services.Abstract;

namespace TelegramReminder.Services
{
    public class CommandsService : ICommandsService
    {
        public Dictionary<string, Func<Context, Task>> _commands = new Dictionary<string, Func<Context, Task>>();

        public CommandsService(IBotService bot, ILogger<CommandsService> logger)
        {
            _commands = new Dictionary<string, Func<Context, Task>>
            {
                { "debug",
                    async (ctx) => await new DebugCmd()
                        .Execute(logger, ctx.AsLocal()?.UseArgument<string>()) },

                { "message",
                    async (ctx) => await ctx.AsLocal()?
                        .UseArgument<string>()
                        .AsMessageTo(ctx.Update.ChatId(), bot.Client) },

                { "title_countdown",
                    async (ctx) => await new TitleCountdownCmd()
                    .Execute(bot, ctx.Update, 
                        args: ctx.AsExternal()?.UseArgument<TitleCountdownArgs>()) },
            };
        }

        public ExecutionContext OneFrom(Context context) =>
            new ExecutionContext(context, _commands.SafeGet(context.Tag));
    }
}
