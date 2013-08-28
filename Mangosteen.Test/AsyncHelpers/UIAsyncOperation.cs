using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Mangosteen.Test
{
    // Our single operation the the queue will keep track of, 
    // then call invoke when it is appropriate.
    internal class UIAsyncOperation : AsyncOperation
    {
        public UIAsyncOperation() {} 

        public UIAsyncOperation(Task action, object state) : base(action, state) { }
        private readonly AutoResetEvent _synchronizingEvent = new AutoResetEvent(false);

        public override void ExecuteAction()
        {
            UISynchronizationContext.Current.OperationStarted();

            Task task;

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            try
            {
                task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () => {
                    try
                    {
                        await _action;
                        tcs.SetResult(new Object());
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }).AsTask();

                SpinWait.SpinUntil(() => task.IsCompleted);
            } catch (Exception)
            {
                int i = 0;
            }
            finally
            {
                UISynchronizationContext.Current.OperationCompleted();
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
