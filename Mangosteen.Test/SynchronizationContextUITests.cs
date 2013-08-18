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
            AsyncHelpers.RealDelay(2000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async Task AsyncTaskUIMethod()
        {
            AsyncHelpers.RealDelay(2000);

            Button b = new Button();
            _shouldBeSet = "Yep it is set.";
        }

        public async void AsyncVoidUIThrowsException()
        {
            AsyncHelpers.RealDelay(2000);

            Button b = new Button();
            throw new Exception("We blew up!");
        }

        public async Task AsyncTaskUIThrowsException()
        {
            AsyncHelpers.RealDelay(2000);

            Button b = new Button();
            throw new Exception("We blew up!");
        }
        #pragma warning restore 1998
        #endregion

        //
        // Tests start here 
        //
        [Fact]
        //#pragma warning disable 1998
        public async Task TryToWait_ForVoidAsync_UsingPost()
        {
            var currentContext = UISynchronizationContext.Register();

            try
            {
                currentContext.Post((t) => AsyncVoidUIMethod(), this);
                currentContext.PumpPendingOperations();

                Assert.True(_shouldBeSet == "Yep it is set.");
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
        #pragma warning restore 1998

        [Fact]
        #pragma warning disable 1998
        ////
        //// Proof that we can call a async void call in a unit test.
        ////
        //public async Task TryToWait_ForVoidAsync_StraightCall()
        //{
        //    AsyncHelpers ah = new AsyncHelpers();
        //    //SynchronizationContext uicontext = await ah.GrabUISynchronizationContext();

        //    var currentContext = new UIAsyncSynchronizationContext();
        //    SynchronizationContext.SetSynchronizationContext(currentContext);

        //    try
        //    {
        //        //currentContext.Post(() => AsyncVoidUIMethod(),this);
        //        currentContext.PumpPendingOperations();

        //        Assert.True(_shouldBeSet == "Yep it is set.");
        //    }
        //    finally
        //    {
        //        SynchronizationContext.SetSynchronizationContext(null);
        //    } 
        //}
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
            await AsyncHelpers.ThrowsExceptionAsync<SynchronizationTestException>(async () =>
            {
                await AsyncTaskUIMethod();
                // The async void here will be put inside of a Func<Task> object so it can be awaited on
                AsyncVoidUIThrowsException();
            });
        }

        [Fact]
        public async Task TryCatchException_TaskException()
        {
            await AsyncHelpers.ThrowsExceptionAsync<SynchronizationTestException>(async () =>
            {
                var currentContext = UISynchronizationContext.Register();
   
                
                try
                {
                    currentContext.Post((t) =>
                    {  
                        AsyncTaskUIThrowsException();
                    
                    }, this);
                    currentContext.PumpPendingOperations();

                    Assert.True(_shouldBeSet == "Yep it is set.");
                }
                catch (Exception)
                {
                    //int i = 0;
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(null);
                }

            });
        }
    }
}
