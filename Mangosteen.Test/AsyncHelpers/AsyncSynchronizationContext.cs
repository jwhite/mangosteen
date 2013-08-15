using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mangosteen.Test
{
    class AsyncSynchronizationContext : SynchronizationContext
    {
        private int _operationCount;
        private readonly AsyncOperationsQueue<AsyncOperation> _operations = new AsyncOperationsQueue<AsyncOperation>();

        // Asynchronous call the the delegate
        // 
        // Send occurs like normal (synchronous) but posts get queue up
        // This should handle the problem with void Tasks?
        public override void Post(SendOrPostCallback d, object state)
        {
            _operations.Enque(new AsyncOperation(d, state));
        }

        public override void OperationCompleted()
        {
            if (Interlocked.Decrement(ref _operationCount) == 0)
            {
                base.OperationCompleted();
            }
        }

        public override void OperationStarted()
        {
            Interlocked.Increment(ref _operationCount);
            base.OperationStarted();
        }

        public void WaitForPendingOperationsToComplete()
        {
            _operations.InvokeAll();
        }
    }
}
