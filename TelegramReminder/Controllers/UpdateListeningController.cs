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
using TelegramReminder.Model.JobSelection;

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
            var message = update?.Message?.Text;
            if (message.IsNullOrEmpty())
                return BadRequest();

            if (!update.HasMentionOf(await _bot.User()))
                return Ok();

            var parseResult = Input.Parse(message);

            if (Input.Errors.Any())
            {
                foreach (var error in Input.Errors)
                    await error.AsMessageTo(update.ChatId(), _bot.Client);

                return Ok();
            }

            await $"Parsed to:\n {parseResult.ToString()}"
                .AsMessageTo(update.ChatId(), _bot.Client);

            var args = new CommandArgs(parseResult.Tag, parseResult.Arguments);

            var job = Jobs
                .FirstSatisfiedBy(args)
                .Select(update, _bot, args);

            await $"Starting a job..."
                .AsMessageTo(update.ChatId(), _bot.Client);

            _jobs.Push(job);
            return Ok();
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

            if (message.IsNullOrEmpty())
                return BadRequest();

            var parseResult = Input.Parse(message);

            if (Input.Errors.Any())
            {
                return Ok();
            }

            var args = new CommandArgs(parseResult.Tag, parseResult.Arguments);

            var job = new LogOutputMessageJob();
            job.WithZone(args.ArgumentOrEmpty("timezone").ToTimeZone());

            _jobs.Push(job);
            return Ok();
        }
    }
}