using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Xunit;

namespace Mangosteen.Test
{
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

        //
        // This works with the above code in a predictable fashion.
        //
        [Fact]
        public async Task TestAsserts_Succeeds()
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
        public async Task TestAsserts_Fails()
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
        private async Task<int> Calculate(int number1, int number2)
        {
            await Task.Delay(2000);
            return number1 + number2;
        }

        //
        // This does not work!!!!  It appears to but it does not.
        //
        // For some reason that I don't understand the await in the line await Calculate(100,100)
        // triggers a return of the above awaiter.GetResult(); on line 38.
        //
        // In otherwords, awaiting in a fuction that is currently being waiting on causes it to think it has been returned from
        // (whew.) and thus the GetResult call on the awaiter ceases to block and everything returns prematurely.
        //
        // I don't understand this at all.
        //
        [Fact]
        public async Task TestAsserts_ContainsAwait()
        {
            await UIThread_Awaitable_Dispatch(async () =>
            {
                var retval = await Calculate(100, 100);

                Assert.True(retval == 200);
                return null;
            });
        }


        //
        // This doesn't work either.  It is exiting early, so looking like it succeeds when it does not.
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
}
