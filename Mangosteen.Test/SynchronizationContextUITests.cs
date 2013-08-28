using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Xunit;



namespace Mangosteen.Test
{
    public class SynchronizationContextUITests
    {
        public class SynchronizationTestException : System.Exception
        {
            public SynchronizationTestException() { }

            public SynchronizationTestException(string message)
                : base(message) { }
        }

        #region Sample async functions to test with ...............................................................
        private string _shouldBeSet;
        //
        // This function has a long delay in it, before setting the variable.
        // This is to insure our test doesn't pass accidentally by satisfying the race condition.
        // 
        // These delays are real, blocking delays that are designed to catch any deadlock situations
        // as well.
        //
        #pragma warning disable 1998
        public async void AsyncVoidUIMethod()
        {
            AsyncHelpers.RealDelay(1000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async Task AsyncTaskUIMethod()
        {
            AsyncHelpers.RealDelay(1000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async Task<object> AsyncTaskReturnsObjectUIMethod()
        {
            AsyncHelpers.RealDelay(1000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";

            return 5;
        }


        public async void AsyncVoidUIThrowsException()
        {
            AsyncHelpers.RealDelay(1000);

            Button b = new Button();
            throw new Exception("We blew up!");
        }

        public async Task AsyncTaskUIThrowsException()
        {
            AsyncHelpers.RealDelay(1000);

            Button b = new Button();
            throw new Exception("We blew up!");
        }
        #pragma warning restore 1998
        #endregion

        //
        // Tests start here 
        //
        [Fact]
        public async Task TryToWait_UsingPost_ForVoidAsync()
        {
            var currentContext = (UISynchronizationContext)UISynchronizationContext.Register();

            currentContext.Post((t) => AsyncVoidUIMethod(), this);
            currentContext.PumpPendingOperations();

            // If the above wasn't awaited, this won't be set yet
            Assert.True(_shouldBeSet == "Yep it is set.");
        }

        [Fact]
        public async Task TryToWait_UsingPost_ForTask()
        {
            var currentContext = (UISynchronizationContext)UISynchronizationContext.Register();

            currentContext.Post((t) => AsyncTaskUIMethod(), this);
            currentContext.PumpPendingOperations();

            Assert.True(_shouldBeSet == "Yep it is set.");
        }

        private Task<object> retval;

        [Fact]
        public async Task TryToWait_UsingPost_ForTaskReturns()
        {
            var currentContext = (UISynchronizationContext)UISynchronizationContext.Register();
            
            currentContext.Post((t) => retval = AsyncTaskReturnsObjectUIMethod(), this);
            currentContext.PumpPendingOperations();

            Assert.True(_shouldBeSet == "Yep it is set.");
            Assert.True((int)retval.Result == 5);
        }


        #pragma warning restore 1998


        //[Fact]
        //// Void function that is passed into a task lambda
        //public async Task TryCatchException_VoidException()
        //{
        //    await AsyncHelpers.ThrowsExceptionAsync<SynchronizationTestException>(async () =>
        //    {
        //        // The async void here will be put inside of a Func<Task> object so it can be awaited on
        //        AsyncVoidUIThrowsException();
        //    });
        //}

        //[Fact]
        // Awaited void function note the async on the lambda to allow for the await
        public async Task TryCatchException_VoidException2()
        {
            //await AsyncHelpers.ThrowsExceptionAsync<SynchronizationTestException>(async () =>
            //{
            //    await AsyncTaskUIMethod();
            //    // The async void here will be put inside of a Func<Task> object so it can be awaited on
            //    AsyncVoidUIThrowsException();
            //});
        }

        [Fact]
        public async Task TryCatchException_TaskException()
        {
            var currentContext = (UISynchronizationContext)UISynchronizationContext.Register();
           
            try
            {
                currentContext.Post((t) => AsyncTaskUIThrowsException(), this);
                currentContext.PumpPendingOperations();
            } catch (Exception e)
            {
                int i = 0;
            }
        }
    }
}
