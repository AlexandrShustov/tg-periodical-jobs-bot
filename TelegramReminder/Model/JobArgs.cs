using System.ComponentModel;

namespace TelegramReminder.Model
{
    public class JobArgs
    {
        public const string CronKey = "j:c";
        public const string AutoRestartKey = "j:ar";

        public string Cron { get; set; }
        public bool AutoRestart { get; set; }
    }
}
