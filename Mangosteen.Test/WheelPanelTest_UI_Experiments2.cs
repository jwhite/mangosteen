using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Xunit;

namespace Mangosteen.Test
{
    public class WheelPanelTest_UI_Experiments2
    {
        // Passing in fuction
        public async Task<object> TestAsserts_Succeeds_refactor(DispatchedHandler action)
        {
            IAsyncAction operation = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);

            // Task completion source proxy so we can return a task to the test
            var tcs = new TaskCompletionSource<object>();

            // Callback from the above RunAsync that will get the exception and stick it in our tcs proxy
            AsyncActionCompletedHandler delegate_completed = delegate(IAsyncAction asyncAction, AsyncStatus asyncStatus)
            {
                // Later we will test the other statuses for approprate returns, but now 
                // we are just trying to solve the exception problem
                if (asyncStatus == AsyncStatus.Error)
                {
                    // Do something here with the exception
                    tcs.SetException(asyncAction.ErrorCode);
                }
                else if (asyncStatus == AsyncStatus.Completed)
                {
                    // TCS seems to need to have a result set, and cannot return NULL
                    tcs.TrySetResult(null);
                }
            };

            operation.Completed = delegate_completed;

            return await tcs.Task;
        }

        [Fact]
        public async Task TestAsserts_Succeeds2()
        {
            await TestAsserts_Succeeds_refactor(() => Assert.True(true));
        }

        [Fact]
        public async Task TestAsserts_Fails2()
        {
            await TestAsserts_Succeeds_refactor(() => Assert.True(false));
        }
    }
}
