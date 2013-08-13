using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xunit;

namespace Mangosteen.Test
{
    public class AsyncHelpersTest
    {
        //
        // This works with the above code in a predictable fashion.
        //
        [Fact]
        public async Task TestAsserts_Should_Succeed()
        {
            await AsyncHelpers.UIThread_Awaitable_Dispatch(() =>
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
            await AsyncHelpers.UIThread_Awaitable_Dispatch(() =>
            {
                Assert.True(false);
                return null;
            });
        }

        [Fact]
        public async Task TestAsserts_ContainsCalculateAwait_ShouldSucceed()
        {
            await AsyncHelpers.UIThread_Awaitable_Dispatch(async () =>
            {
                var retval = await AsyncHelpers.SlowCalculate(100, 100);

                Assert.True(retval == 200);
                return null;
            });
        }

        [Fact]
        public async Task TestAsserts_ContainsCalculateAwait_ShouldFail()
        {
            await AsyncHelpers.UIThread_Awaitable_Dispatch(async () =>
            {
                var retval = await AsyncHelpers.SlowCalculate(100, 100);

                Assert.True(retval == 500);
                return null;
            });
        }

        [Fact]
        public async Task TestAsserts_Contains_UIUpdate_ShouldFail()
        {
            // Lets call this Awaiting thread A.
            await AsyncHelpers.UIThread_Awaitable_Dispatch(async () =>
            {
                Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

                Grid _grid;

                _grid = new Grid { Width = 800, Height = 800 };
                nonVisibleMainWindow.Content = _grid;
                nonVisibleMainWindow.Activate();

                Debug.WriteLine("About to wait for update layout.");
                object x = await AsyncHelpers.UpdateLayoutAsync(_grid);

                //
                // Everything below here never gets exectued, as when the await occurs a line above
                // it lets go at "back in the test body"
                //
                // Why oh why?
                //
                Debug.WriteLine("Done waiting for update layout.");

                Task.Delay(2000).Wait();    // <- This will block this thread as intended.. Nice long delay 

                Debug.WriteLine("About to throw assert!");
                Assert.True(false);

                return null;
            });

            Debug.WriteLine("Back in the test body after waiting on the UI thread functionality.");
            Debug.WriteLine("About to exit the test");
        }

        [Fact]
        public async Task TestAsserts_Contains_SimpleAsync_ShouldFail()
        {
            await AsyncHelpers.UIThread_Awaitable_Dispatch(async () =>
            {
                Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

                Grid _grid;

                _grid = new Grid { Width = 800, Height = 800 };
                nonVisibleMainWindow.Content = _grid;
                nonVisibleMainWindow.Activate();

                await AsyncHelpers.SuperSimpleAsyncFunction();

                Assert.True(false);

                return null;
            });
        }

        [Fact]
        public async Task TestAsserts_Contains_SimpleAsync_ShouldSucceed()
        {
            await AsyncHelpers.UIThread_Awaitable_Dispatch(async () =>
            {
                Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

                Grid _grid;

                _grid = new Grid { Width = 800, Height = 800 };
                nonVisibleMainWindow.Content = _grid;
                nonVisibleMainWindow.Activate();

                await AsyncHelpers.SuperSimpleAsyncFunction();

                Assert.True(true);

                return null;
            });
        }
    }
}
