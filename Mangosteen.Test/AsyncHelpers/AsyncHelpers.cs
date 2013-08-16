using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Xunit;


namespace Mangosteen.Test
{
    public class AsyncHelpers
    {
        //
        // Good delay for testing debugging of async calls because it will *block* this thread.
        //
        public static void RealDelay(int miliseconds)
        {
            Task.Delay(miliseconds).Wait();
        }

        #pragma warning disable 1998  // Intentioanlly don't have await
        public static async Task SuperSimpleAsyncFunction()
        {
            RealDelay(2000);
            return;
        }

        //
        // Simplest async task I could come up with to test my tests...
        //
        #pragma warning disable 1998    // Intentioanlly don't have await
        public static async Task<int> SlowCalculate(int number1, int number2)
        {
            RealDelay(2000);
            return number1 + number2;
        }

        //
        // This is tested and works
        //
        private SynchronizationContext _localcontext;
        public async Task<SynchronizationContext> GrabUISynchronizationContext()
        {
            // This is how microsoft handles awaiting the task that is passed into runAsync
            // I have deconstructed it a little for the simple case.
            Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => { 
                    _localcontext = SynchronizationContext.Current;
                }).AsTask();

            TaskAwaiter awaiter = task.GetAwaiter();

            if (!task.IsCompleted)
            {
                await task;
            }

            return _localcontext;
        }

        public static async Task ThrowsExceptionAsync<TException>(Func<Task> func) where TException : Exception
        {
            // Save of the current context in case it is not null
            // But, in the case of tests it will probably always be null
            var savedContext = SynchronizationContext.Current;

            var currentContext = new AsyncSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(currentContext);

            try
            {
                await func.Invoke();
                currentContext.PumpPendingOperations();
            } 
            catch (TException)
            {
                return;
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(savedContext);
            }

            Assert.True(false, "Delegate did not throw " + typeof(TException).Name);
        }
    }
}
