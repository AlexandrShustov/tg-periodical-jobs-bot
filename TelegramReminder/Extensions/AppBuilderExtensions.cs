using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using TelegramReminder.Model.Concrete;
using TelegramReminder.Model.Exceptions;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Extensions
{
    public  static class AppBuilderExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, object bot)
        {
            app.UseExceptionHandler(appError =>
            {
                //appError.Run(async context =>
                //{
                //    var error = context.Features.Get<IExceptionHandlerPathFeature>();
                //    var telegramException = error.Error as CommandLogicException;

                //    if (telegramException is null)
                //        return;

                //    if (telegramException.Update == null)
                //        return;

                //    await error.Error.Message
                //        .AsMessageTo(telegramException.Update.ChatId(), bot.Client);
                //});
            });
        }
    }
}
