using System.Threading.Tasks;
using TelegramReminder.Model;

namespace TelegramReminder.Services.Abstract
{
    public interface ICommandsService
    {
        ExecutionContext OneFrom(Context context);
    }
}
