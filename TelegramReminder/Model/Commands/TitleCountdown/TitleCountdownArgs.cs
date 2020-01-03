using System;

namespace TelegramReminder.Model.Commands.TitleCountdown
{
    public class TitleCountdownArgs
    {
        [Argument("cron", required: true)]
        public string Cron { get; set; }

        [Argument("message", required: true)]
        public string Message { get; set; }

        [Argument("deadline", required: true)]
        public DateTime Deadline { get; set; }

        [Argument("autorestart", required: false)]
        public bool Autorestart { get; set; }

        [Argument("timezone", required: true)]
        public TimeZoneInfo TimeZone { get; set; }
    }
}
