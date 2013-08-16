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
        private readonly AsyncOperationsPump<AsyncOperation> _operations = new AsyncOperationsPump<AsyncOperation>();

        // Asynchronous call the the delegate
        // 
        // Send occurs like normal (synchronous) but posts get queue up
        // This should handle the problem with void Tasks?
        public override void Post(SendOrPostCallback d, object state)
        {
            _operations.ScheduleOperation(new AsyncOperation(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            base.Send(d, state);
        }

        public override void OperationCompleted()
        {
            if (Interlocked.Decrement(ref _operationCount) == 0)
            {
                _operations.SetPumpFinished();
            }

            base.OperationCompleted();
        }

        public override void OperationStarted()
        {
            Interlocked.Increment(ref _operationCount);
            base.OperationStarted();
        }

        public void PumpPendingOperations()
        {
            _operations.ContinuousPump();
        }
    }
}
