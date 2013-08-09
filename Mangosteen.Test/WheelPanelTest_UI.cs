using Mangosteen.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using WinRTXamlToolkit.AwaitableUI;
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTest_UI
    {
        public async Task<object> TestAsserts_Succeeds_Refactor(DispatchedHandler action)
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

        private static async Task AwaitableUpdate(FrameworkElement element)
        {
            element.UpdateLayout();

            await EventAsync.FromEvent<object>(
                eh => element.LayoutUpdated += eh,
                eh => element.LayoutUpdated -= eh
                );
        }

        private static WheelPanel CreateAndHostPanel()
        {
            Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

            Grid _grid;
            WheelPanel p;

            _grid = new Grid { Width = 800, Height = 800 };
            nonVisibleMainWindow.Content = _grid;
            nonVisibleMainWindow.Activate();

            p = new WheelPanel();

            _grid.Children.Add(p);

            return p;
        }

        [Fact]
        public async Task WheelPanel_Can_Be_Hosted()
        {
            await TestAsserts_Succeeds_Refactor(() =>
            {
                var panel = CreateAndHostPanel();

                Assert.True(panel != null);
            });
        }

        [Theory]
        [InlineData(50, 100, 100)]
        [InlineData(200, 100, 100)]
        public async Task Changing_Size_Does_Not_Change_Radius_If_Set(double widthheight, double outerradius, double value)
        {
            await TestAsserts_Succeeds_Refactor(async () =>
            {
                var panel = CreateAndHostPanel();

                panel.Width = 100;
                panel.Height = 100;

                await AwaitableUpdate(panel);

                Assert.True(panel.ActualRadius == 50);
                
                panel.OuterRadius = outerradius;

                panel.Width = widthheight;
                panel.Height = widthheight;

                await AwaitableUpdate(panel);

                Assert.True(panel.ActualRadius == value);
            });
        }

        [Theory]
        [InlineData(100, 100, 50)]
        [InlineData(200, 200, 100)]
        [InlineData(100, 50, 25)]
        [InlineData(50, 100, 25)]
        //
        // Width and height should set the radius unless an outerradius is defined
        //
        public async Task Width_Height_Sets_Actual_Radius(double width, double height, double value)
        {
            await TestAsserts_Succeeds_Refactor(async () =>
            {
                var panel = CreateAndHostPanel();

                panel.Width = width;
                panel.Height = height;

                await AwaitableUpdate(panel);

                Assert.True(panel.ActualRadius == value);
            });
        }

        [Theory]
        [InlineData(100, 100, 50, 50)]
        [InlineData(200, 200, 100, 100)]
        [InlineData(100, 50, 50, 25)]
        [InlineData(50, 100, 25, 50)]
        //
        // Center should always be at the center of the width and height box.
        //
        public async Task Width_Height_Sets_Center(double width, double height, double centerx, double centery)
        {
            await TestAsserts_Succeeds_Refactor(async () =>
            {
                var panel = CreateAndHostPanel();
                
                panel.Width = width;
                panel.Height = height;

                await AwaitableUpdate(panel);

                Assert.True((panel.Center.X == centerx) && (panel.Center.Y == centery));
            });
        }

        [Fact]
        public async Task Can_Add_4_Children()
        {
            await TestAsserts_Succeeds_Refactor(async () =>
            {
                var panel = CreateAndHostPanel();

                Button[] buttons = new Button[4];
                for (int i = 0; i < 4; i++)
                {
                    Button b = new Button();
                    b.Name = String.Format("Button {0}", i);
                    panel.Children.Add(b);
                }

                await AwaitableUpdate(panel);

                Assert.True(panel.Children.Count == 4);
                Assert.True(panel.Children[0].GetType() == typeof(Button));
            });
        }
    }
}
