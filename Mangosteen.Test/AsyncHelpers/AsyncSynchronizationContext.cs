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

        public static SynchronizationContext Register()
        {
            SynchronizationContext synchronizationContext = null;

            if (Current == null)
            {
                //
                // Case where current context is null
                //
                synchronizationContext = new AsyncSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            }
            else
            {
                //
                // Case where we have an existing context, for now just overwrite it.
                // But at some point special handling will occur here to preserve the old one.
                synchronizationContext = new AsyncSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            }

            // Register for unobserved exceptions
            // TODO : We may need to hook this on the UI thread in a better way then this?
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            return synchronizationContext;
        }

        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //throw new NotImplementedException();
            e.SetObserved();
            throw e.Exception;
        }

        // Asynchronous call the the delegate
        // 
        // Send occurs like normal (synchronous) but posts get queue up
        // This should handle the problem with void Tasks?
        public override void Post(SendOrPostCallback d, object state)
        {
            Action<object> a = new Action<object>(d);
            Task t = new Task(a, state);
             _operations.ScheduleOperation(new AsyncOperation(t, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {

            base.Send(d, state);
        }

        public override void OperationCompleted()
        {
            if (_operationCount == 0) throw new Exception("Thread completed but was never started!");

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
