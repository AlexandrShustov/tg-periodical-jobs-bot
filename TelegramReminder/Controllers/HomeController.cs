using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramReminder.Model;

namespace TelegramReminder.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [Route("api/home")]
        public IActionResult Index() =>
            Ok("Alive");
    }
}