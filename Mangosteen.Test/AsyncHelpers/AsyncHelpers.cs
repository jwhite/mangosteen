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

        public static SynchronizationContext GrabUISynchronizationContext()
        {
            var helper = new AsyncHelpers();
            Task<SynchronizationContext> t = helper.GrabUISynchronizationContextAsync();
            t.Wait();
            return t.Result;
        }

        private SynchronizationContext _localcontext;
        private async Task<SynchronizationContext> GrabUISynchronizationContextAsync()
        {
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


        public static CoreDispatcher GrabUIDispatcher()
        {
            var helper = new AsyncHelpers();
            Task<CoreDispatcher> t = helper.GrabUIDispatcherAsync();
            t.Wait();
            return t.Result;
        }
        //
        // This is tested and works
        //
        private CoreDispatcher _localDispatcher;
        private async Task<CoreDispatcher> GrabUIDispatcherAsync()
        {
            // This is how microsoft handles awaiting the task that is passed into runAsync
            // I have deconstructed it a little for the simple case.
            Task task = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => {
                    _localDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher; 
                }).AsTask();

            TaskAwaiter awaiter = task.GetAwaiter();

            if (!task.IsCompleted)
            {
                await task;
            }

            return _localDispatcher;
        }

        public static async Task ThrowsExceptionAsync<TException>(Task func) where TException : Exception
        {
            // Save of the current context in case it is not null
            // But, in the case of tests it will probably always be null
            var currentContext = (AsyncSynchronizationContext)AsyncSynchronizationContext.Register();

            try
            {
                //Task task = func;
                //currentContext.Post((t) =>
                //{
                //    task.Start();

                //    task.ContinueWith((task) => { });
                //    }, null);

                
                //currentContext.PumpPendingOperations();
           

            } 
            catch (TException)
            {
                return;
            }
            finally
            {
                //SynchronizationContext.SetSynchronizationContext(savedContext);
            }

            Assert.True(false, "Delegate did not throw " + typeof(TException).Name);
        }

        public delegate Task<object> ActionDelegate();

        
    }
}
