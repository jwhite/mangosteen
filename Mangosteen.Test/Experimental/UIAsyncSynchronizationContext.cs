using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Mangosteen.Test
{
    class UIAsyncSynchronizationContext : SynchronizationContext
    {
        private int _operationCount;
        private readonly AsyncOperationsPump<UIAsyncOperation> _operations = new AsyncOperationsPump<UIAsyncOperation>();

        // Asynchronous call the the delegate
        // 
        // Send occurs like normal (synchronous) but posts get queue up
        // This should handle the problem with void Tasks?
        public override void Post(SendOrPostCallback d, object state)
        {
            try
            {
                _operations.ScheduleOperation(new UIAsyncOperation(d, state));
            } catch(Exception e)
            {
                int i = 0;
            }
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            try
            {
                base.Send(d, state);
            }
            catch (Exception e)
            {
                int i = 0;
            }
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
