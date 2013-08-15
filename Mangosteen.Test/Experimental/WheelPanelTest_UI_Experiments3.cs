using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Xunit;

namespace Mangosteen.Test
{
#if false
    public class WheelPanelTest_UI_Experiments3
    {
        // This is a delegate that exists so I can pass in a lambda in the form of () => { do some stuff; }
        // Unfortunately, because it has to be Task<object> so that there is a return value, the 
        // passed in lambda MUST have a return null at the end of it.
        //
        // Haven't found a workaround for this yet.
        public delegate Task<object> ActionDelegate();

        public static Task<object> UIThread_Awaitable_Dispatch(ActionDelegate expression)
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

                TaskAwaiter awaiter = task.GetAwaiter();

                if (!task.IsCompleted)
                {
                    // This will block, awaiting the return from the above task
                    SpinWait.SpinUntil(() => awaiter.IsCompleted);

                    //
                    // Unfortunately, if the above task 'expression' contains an await, for some reason that I don't understand, this will 
                    // return.  How do I fix that?
                    awaiter.GetResult();

                    // TCS seems to need to have a result set, 
                    if (awaiter.IsCompleted)
                    {
                        if (retval == null || !retval.IsFaulted)
                        {
                            // This block will execute on the Assert.True(true) case
                            // or any time there is no assertion.
                            tcs.TrySetResult(retval);
                        }
                        else if (retval.IsFaulted)
                        {
                            tcs.TrySetException(retval.Exception);
                        }
                    }
                }
                else
                {
                    // This is the case where the task completes immediately
                    if (!task.IsFaulted)
                    {
                        tcs.TrySetResult(retval);
                    }
                    else
                    {
                        tcs.TrySetException(task.Exception);
                    }
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

        //
        // This works with the above code in a predictable fashion.
        //
        [Fact]
        public async Task TestAsserts_Should_Succeed()
        {
            await UIThread_Awaitable_Dispatch(() => 
            { 
                Assert.True(true); 
                return null; 
            });
        }

        //
        // This works with the above code in a predictable fashion.
        //
        [Fact]
        public async Task TestAsserts_Should_Fail()
        {
            await UIThread_Awaitable_Dispatch(() =>
            {
                Assert.True(false);
                return null;
            });
        }


        //
        // Simplest async task I could come up with to test my tests...
        //
        #pragma warning disable 1998 // Intentioanlly don't have await
        private async Task<int> Calculate(int number1, int number2)
        {
            Task.Delay(2000).Wait();

            return number1 + number2;
        }

        [Fact]
        public async Task TestAsserts_ContainsAwait_ShouldSucceed()
        {
            await UIThread_Awaitable_Dispatch(async () =>
            {
                var retval = await Calculate(100, 100);

                Assert.True(retval == 200);
                return null;
            });
        }

        //
        // This doesn't work either
        //
        [Fact]
        public async Task TestAsserts_ContainsAwait_ShouldFail()
        {
            await UIThread_Awaitable_Dispatch(async () =>
            {
                var retval = await Calculate(100, 100);

                Assert.True(retval == 500);
                return null;
            });
        }
    }
#endif
}
