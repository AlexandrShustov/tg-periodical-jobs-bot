using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramReminder.Model;
using TelegramReminder.Model.Concrete;
using TelegramReminder.Model.Extensions;

namespace TelegramReminder.Controllers
{
    [ApiController]
    public class UpdateListeningController : Controller
    {
        private readonly TelegramBot _bot;

        public UpdateListeningController(TelegramBot bot)
        {
            _bot = bot;
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

            if (_bot.CanExecute(args))
                await _bot.Execute(update, args);
            else
                await $"Couldn`t execute command".AsMessageTo(update.ChatId(), _bot.Client);

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
                foreach (var error in Input.Errors)
                    continue;

                return Ok();
            }

            var args = new CommandArgs(parseResult.Tag, parseResult.Arguments);

            if (_bot.CanExecute(args))
                await _bot.Execute(null, args);

            return Ok();
        }
    }
}