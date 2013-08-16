using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Xunit;



namespace Mangosteen.Test
{
    public class SynchronizationContextTests
    {
        [Fact]
        public void CannotGetSynchronizationContextInTest()
        {
            SynchronizationContext localContext;
            localContext = SynchronizationContext.Current;

            Assert.True(localContext == null);
        }

        [Fact]
        public async Task TryToGrabSynchronizationContextFromUI()
        {
            AsyncHelpers ah = new AsyncHelpers();

            SynchronizationContext uicontext = await ah.GrabUISynchronizationContext();

            Assert.True(uicontext != null);
            Assert.True(uicontext is System.Threading.SynchronizationContext);
        }

        private string _shouldBeSet;
        //
        // This function has a long delay in it, before setting the variable.
        // This is to insure our test doesn't pass accidentally by satisfying the race condition.
        //
        #pragma warning disable 1998
        public async void AsyncVoidMethod()
        {
            AsyncHelpers.RealDelay(2000);
            _shouldBeSet = "Yep it is set.";
        }
        #pragma warning restore 1998


        [Fact]
        #pragma warning disable 1998
        public async Task TryToWait_ForAVoidAsync_UsingPost()
        {
            var currentContext = new AsyncSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(currentContext);

            try
            {
                currentContext.Post((t) => AsyncVoidMethod(),this);
                currentContext.PumpPendingOperations();

                Assert.True(_shouldBeSet == "Yep it is set.");
            } finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            } 
        }
        #pragma warning restore 1998

        [Fact]
        #pragma warning disable 1998
        public async Task TryToWait_ForAVoidAsync_StraightCall()
        {
            var currentContext = new AsyncSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(currentContext);

            try
            {
                AsyncVoidMethod();
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
        public async Task TryCatchException_()
        {

        }
#pragma warning restore 1998
    }
}
