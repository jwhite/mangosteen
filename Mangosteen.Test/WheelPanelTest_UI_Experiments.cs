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
    public class WheelPanelTest_UI_Experiments
    {
        /// <summary>
        /// This works!  It exectues on the UI thread and captures asserts properly!
        /// Now I need to refactor it!
        /// </summary>
        /// <returns></returns>
#if false

        [Fact]
        public async Task<object> TestAsserts_Fails()
        {
            IAsyncAction operation = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Simulated unit test that runs on the UI and eventually throws
                // an exception.
                Assert.True(false);
            });

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
        public async Task<object> TestAsserts_Succeeds()
        {
            IAsyncAction operation = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Simulated unit test that runs on the UI and eventually throws
                // an exception.
                Assert.True(true);
            });

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

        //
        // The above works, refactoring below....
        // 

        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        public static Task<object> ExecuteUIAsyncWithAsserts<TResult>(Task<object> task)
        {
            IAsyncAction operation = ExecuteOnUIThread(task.AsAsyncAction());

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

            return tcs.Task;
        }

        
        
#endif
    }
}
