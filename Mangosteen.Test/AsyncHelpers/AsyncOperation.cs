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
        protected readonly SendOrPostCallback _action;
        protected readonly object _state;

        public AsyncOperation() {}

        public AsyncOperation(SendOrPostCallback action, object state)
        {
            _action = action;
            _state = state;
        }

        public virtual void Invoke()
        {
            _action(_state);
        }
    }
}
