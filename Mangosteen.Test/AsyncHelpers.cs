using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Mangosteen.Test
{
    public static class AsyncHelpers
    {
        //
        // Microsoft solves this problem of awaiting a RunAsync call by converting to a task, then to an awaiter, and using that

        // For some reason that I don't understand, if there is an await call 
        public delegate Task<object> ActionDelegate();

        public static Task<object> UIThread_Awaitable_Dispatch(ActionDelegate expression)
        {
            try
            {
                object retval = null;

                // This is how microsoft handles awaiting the task that is passed into runAsync
                // I have deconstructed it a little for the simple case.
                Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => retval = expression())
                    .AsTask();

                TaskAwaiter awaiter = task.GetAwaiter();

                // This will block, awaiting the return from the above task
                //
                // Unfortunately, if the above task 'expression' contains an await, for some reason that I don't understand, this will 
                // return.  How do I fix that?
                awaiter.GetResult();

                // If there hasn't been a task assertion, need to build a task completion source to return any results
                // Task completion source proxy so we can return a task to the test

                // In other words, this task completion source is a proxy that exists to hold return values.
                var tcs = new TaskCompletionSource<object>();

                // TCS seems to need to have a result set, 
                if (awaiter.IsCompleted)
                {
                    // This block will execute on the Assert.True(true) case
                    // or any time there is no assertion.
                    tcs.TrySetResult(retval);

                    return tcs.Task;
                }
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

            return null;
        }
    }
}
