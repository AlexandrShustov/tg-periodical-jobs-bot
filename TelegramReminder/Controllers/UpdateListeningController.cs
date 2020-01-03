using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramReminder.Model;
using TelegramReminder.Model.Extensions;
using TelegramReminder.Services.Abstract;

namespace TelegramReminder.Controllers
{
    [ApiController]
    public class UpdateListeningController : Controller
    {
        private readonly ICommandsService _commands;

        public UpdateListeningController(ICommandsService commandsService) =>
            _commands = commandsService;

        [HttpPost]
        [Route("api/bot/update")]
        public async Task<IActionResult> Update([FromBody]Update update)
        {
            var message = update?.Message?.Text;
            if (message.IsNullOrEmpty())
                return BadRequest();

            var parsed = Input.Parse(message);

            if (Input.Errors.Any())
            {
                foreach (var error in Input.Errors)
                    await _commands
                        .OneFrom(new Context("message", update).UseLocal(with: error))
                        .Execute();

                return Ok();
            }

            await _commands
                .OneFrom(new Context("message", update).UseLocal(with: $"Parsed to:\n {parsed.ToString()}"))
                .Execute();

            await _commands
                .OneFrom(new Context(parsed.Tag, update).UseExternal(parsed.Arguments))
                .Execute();
            
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

            var parseResult = Input.Parse(message);

            if (Input.Errors.Any())
            {
                foreach (var error in Input.Errors)
                    await _commands
                        .OneFrom(new Context("debug", null).UseLocal(with: error))
                        .Execute();
                       
                return Ok();
            }

            var context = new Context("debug", null)
                .UseLocal(with: $"Parsed to:\n {parseResult.ToString()}");

            await _commands
                .OneFrom(context)
                .IfError(e => { })
                .Execute();

            return Ok();
        }
    }
}