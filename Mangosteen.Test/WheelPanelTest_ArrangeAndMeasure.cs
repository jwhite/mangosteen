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
using WinRTXamlToolkit.AwaitableUI;
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTest_ArrangeAndMeasure
    {
        private static async Task AwaitableUpdate(FrameworkElement element)
        {
            element.UpdateLayout();

            await EventAsync.FromEvent<object>(
                eh => element.LayoutUpdated += eh,
                eh => element.LayoutUpdated -= eh
                );
        }

        private static WheelPanel CreateAndHostPanel_180Degrees_ChildButtons()
        {
            Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;

            Grid _grid;
            WheelPanel p;

            _grid = new Grid { Width = 800, Height = 800 };
            nonVisibleMainWindow.Content = _grid;
            nonVisibleMainWindow.Activate();

            p = new WheelPanel();
            p.StartAngle = 0;
            p.EndAngle = 180;
            
            _grid.Children.Add(p);

            Button[] buttons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                Button b = new Button();
                b.Name = String.Format("Button {0}", i);
                p.Children.Add(b);
            }

            return p;
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

        /// <summary>
        /// This works!  It exectues on the UI thread and captures asserts properly!
        /// </summary>
        /// <returns></returns>
        //[Theory]
        //[InlineData(50)]
        //public Task<object> Test_4_Even_Star_Sized(double size)
        //{
        //    IAsyncAction operation = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => 
        //    {
        //        var panel = CreateAndHostPanel_180Degrees_ChildButtons();

        //        await AwaitableUpdate(panel);

        //        Assert.True(false);
        //    });
            
        //    // Task completion source proxy so we can return a task to the test
        //    var tcs = new TaskCompletionSource<object>();

        //    // Callback from the above RunAsync that will get the exception and stick it in our tcs proxy
        //    AsyncActionCompletedHandler delegate_completed = delegate(IAsyncAction asyncAction, AsyncStatus asyncStatus)
        //    {
        //        // Later we will test the other statuses for approprate returns, but now 
        //        // we are just trying to solve the exception problem
        //        if (asyncStatus == AsyncStatus.Error)
        //        {
        //            // Do something here with the exception
        //            tcs.SetException(asyncAction.ErrorCode);
        //        }
        //        else if (asyncStatus == AsyncStatus.Completed)
        //        {
        //            // TCS seems to need to have a result set, and cannot return NULL
        //            tcs.TrySetResult(null);
        //        }
        //    };

        //    operation.Completed = delegate_completed;

        //    tcs.Task;
        //}
    }
}
