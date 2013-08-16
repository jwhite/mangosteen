using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Mangosteen.Test
{
    class OldAsyncHelpers
    {
        // This is a delegate that exists so I can pass in a lambda in the form of () => { do some stuff; }
        // Unfortunately, because it has to be Task<object> so that there is a return value, the 
        // passed in lambda MUST have a return null at the end of it.
        //
        // Haven't found a workaround for this yet.
        public delegate Task<object> ActionDelegate();

        //
        // This does NOT work.
        //
        public static async Task<object> UIThread_Awaitable_Dispatch(ActionDelegate expression)
        {
            try
            {
                // If there hasn't been a task assertion, need to build a task completion source to return any results
                // Task completion source proxy so we can return a task to the test
                // In other words, this task completion source is a proxy that exists to hold return values.
                var tcs = new TaskCompletionSource<object>();

                Task<object> retval = null;

                // This is how microsoft handles awaiting the task that is passed into runAsync
                // I have deconstructed it a little for the simple case.
                Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => retval = expression())
                    .AsTask();

                if (!task.IsCompleted)
                {
                    await task;
                }

                Debug.WriteLine("Finished waiting on lambda running on UI thread because it either threw exception or returned");

                // This is the case where the task completes immediately
                // if retval is null, the task ran and completed instantly
                // Not sure why in this case retval is null but it is.
                if ((retval == null) || !retval.IsFaulted)
                {
                    tcs.TrySetResult(retval);
                }
                else
                {
                    tcs.TrySetException(retval.Exception);
                }

                return tcs.Task;
            }
            catch (Exception e)
            {
                // This will trap any exceptions that occur in the above 
                // expression function.

                // We need to fill up a task completion source to return
                var tcs = new TaskCompletionSource<object>();

                tcs.TrySetException(e);

                return tcs.Task;
            }
        }

        public static Task<object> UpdateLayoutAsync(FrameworkElement element)
        {
            var tcs = new TaskCompletionSource<object>();

            EventHandler<object> handler = null;
            handler = delegate
            {
                Debug.WriteLine("Update layout event has occured.");
                element.LayoutUpdated -= handler;
                tcs.TrySetResult(null);
            };

            element.LayoutUpdated += handler;

            element.UpdateLayout();

            return tcs.Task;
        }
    }
}
