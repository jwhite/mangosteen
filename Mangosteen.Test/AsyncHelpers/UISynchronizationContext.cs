using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mangosteen.Test
{
    public class UISynchronizationContext : SynchronizationContext
    {
        public static SynchronizationContext WinRTUIContext;
        private int _operationCount;
        private readonly AsyncOperationsPump<UIAsyncOperation> _operations = new AsyncOperationsPump<UIAsyncOperation>();
        
        public UISynchronizationContext()
        {
         }

        public UISynchronizationContext(SynchronizationContext ui)
        {
            WinRTUIContext = ui;
        }

        public static UISynchronizationContext Register()
        {
            UISynchronizationContext uiSynchronizationContext = null;

            if (Current == null)
            {
                //
                // Case where current context is null
                //
                uiSynchronizationContext = new UISynchronizationContext(AsyncHelpers.GrabUISynchronizationContext());
                SynchronizationContext.SetSynchronizationContext(uiSynchronizationContext);
            }
            else if (Current.GetType().FullName == "System.Threading.WinRTSynchronizationContext")
            {
                //
                // Case where current context is the UI context
                //
                uiSynchronizationContext = new UISynchronizationContext(Current);
            } 
            else 
            {
                //
                // Case where we have an existing context, for now just overwrite it.
                // But at some point special handling will occur here to preserve the old one.
                uiSynchronizationContext = new UISynchronizationContext(AsyncHelpers.GrabUISynchronizationContext());
                SynchronizationContext.SetSynchronizationContext(uiSynchronizationContext);
            }

            // Register for unobserved exceptions
            // TODO : We may need to hook this on the UI thread in a better way then this?
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            
            return uiSynchronizationContext;
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
            _operations.ScheduleOperation(new UIAsyncOperation(d, state));
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
