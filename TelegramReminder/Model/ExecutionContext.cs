using System;
using System.Threading.Tasks;

namespace TelegramReminder.Model
{
    public class ExecutionContext
    {
        private Context _context;

        private Func<Context, Task> _action;
        private Action<Exception> _onError;

        public ExecutionContext(Context context, Func<Context, Task> action)
        {
            _context = context;
            _action = action;
        }

        public ExecutionContext IfError(Action<Exception> onError)
        {
            _onError = onError;
            return this;
        }

        public async Task Execute()
        {
            try
            {
                if (_action == null)
                    throw new ArgumentNullException();

                await _action(_context);
            }
            catch (Exception e)
            {
                _onError?.Invoke(e);
            }
        }
    }
}
