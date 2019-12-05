using System.Collections.Generic;
using System.Linq;
using TelegramReminder.Model.JobSelection.Concrete;

namespace TelegramReminder.Model.JobSelection
{
    public static class Jobs
    {
        private static IEnumerable<JobSelector> Selectors =
            new List<JobSelector> { new ChangeTitleJobSelector() };

        public static JobSelector FirstSatisfiedBy(CommandArgs args) =>
            Selectors.FirstOrDefault(s => s.SatisfiedBy(args));
    }
}
