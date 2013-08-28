using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mangosteen.Test
{
    // Our single operation the the queue will keep track of, 
    // then call invoke when it is appropriate.
    internal class AsyncOperation
    {
        protected readonly Task _action;
        protected readonly object _state;

        public AsyncOperation() {}

        public AsyncOperation(Task action, object state)
        {
            _action = action;
            _state = state;
        }

        public virtual void ExecuteAction()
        {
            try
            {
                var awaiter = _action.GetAwaiter();
                _action.Start();

                SpinWait.SpinUntil(() => _action.IsCompleted);

            } catch (Exception e)
            {
                int i = 0;
            }
        }
    }
}
