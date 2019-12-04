using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramReminder.Model;
using TelegramReminder.Model.Abstract;
using TelegramReminder.Model.Concrete;
using TelegramReminder.Model.Extensions;
using TelegramReminder.Model.Jobs;

namespace TelegramReminder.Controllers
{
    [ApiController]
    public class UpdateListeningController : Controller
    {
        private readonly TelegramBot _bot;
        private readonly IPeriodicalJobs _jobs;

        public UpdateListeningController(TelegramBot bot, IPeriodicalJobs jobs)
        {
            _bot = bot;
            _jobs = jobs;
        }

        [HttpPost]
        [Route("api/bot/update")]
        public async Task<IActionResult> Update([FromBody]Update update)
        {
            if (await HasBotMention(update) is false)
                return Ok();

            var message = update.Message.Text;
            var result = CommandParser.Parse(message);
            await $"Parsed to:\n {result.ToString()}".AsMessageTo(update.ChatId(), _bot.Client);  

            if (!result.IsValid)
            {
                await $"Invalid command or anything else".AsMessageTo(update.ChatId(), _bot.Client);
                return Ok();
            }

            await result.Command
                .IfNullOrEmpty($"Cant recognize a command"
                    .AsMessageTo(update.ChatId(), _bot.Client));

            var cron = result.ArgumentOrEmpty("cron");
            await cron
                .IfNullOrEmpty($"Cant recognize a Cron expression"
                    .AsMessageTo(update.ChatId(), _bot.Client));

            var autorestart = result.ArgumentOrEmpty("autorestart").ToBool();
            var timezone = result.ArgumentOrEmpty("timezone").ToTimeZone();

            var args = new CommandArgs() { Tag = result.Command, Args = result.Arguments };
            var job = new TelegramJob(update, _bot, args)
                .WithCrone(cron)
                .WithAutoRestart(autorestart)
                .WithTimeZone(timezone);

            await $"Starting a job...".AsMessageTo(update.ChatId(), _bot.Client);

            _jobs.Push(job);
            return Ok();
        }

        private async Task<bool> HasBotMention(Update update)
        {
            var botUser = await _bot.Client.GetMeAsync(); 

            if(update.Message?.Entities.Any(e => e.Type == Telegram.Bot.Types.Enums.MessageEntityType.Mention) == true)
            {
                return update.Message.Text.Contains(botUser.Username);
            }

            return false;
        }

        [HttpPost]
        [Route("api/bot/updateTEST")]
        public async Task<IActionResult> UpdateTest()
        {
            string message;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                message = await reader.ReadToEndAsync();
            }

            var result = CommandParser.Parse(message);

            if (!result.IsValid)
            {
                return BadRequest();
            }

            var args = new CommandArgs() { Tag = result.Command, Args = result.Arguments };
            var hasCron = result.Arguments.TryGetValue("cron", out var cron);

            if (hasCron is false)
            {
                return Ok();
            }

            var hasAutoRestart = result.Arguments.TryGetValue("autorestart", out var autoRestart);
            var shouldAutoRestart = hasAutoRestart is false
                ? false
                : Convert.ToBoolean(autoRestart);

            var hasTimeZone = result.Arguments.TryGetValue("timezone", out var timeZoneString);

            TimeZoneInfo timeZone = null;
            if (hasTimeZone)
            {
                try
                {
                    timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
                }
                catch (Exception ex)
                {
                    //await _bot.Client.SendTextMessageAsync(update.Message.Chat.Id, $"Error {ex.Message}");
                    return Ok();
                }
            }

            var job = new LogOutputMessageJob();
            job.WithZone(timeZone);
            _jobs.Push(job);

            return Ok();
        }
    }
}