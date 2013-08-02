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
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTest_UI
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

#if false
        [Fact]
        public async Task WheelPanel_Can_Be_Hosted__attempt_crashes_tester()
        {
            Grid grid = null;

            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Window nonVisibleMainWindow = Windows.UI.Xaml.Window.Current;
                grid = new Grid { Width = 800, Height = 800 };
                nonVisibleMainWindow.Content = grid;
                //nonVisibleMainWindow.Activate();
                //WheelPanel panel = new WheelPanel();
                //grid.Children.Add(panel);
                //grid.UpdateLayout();

                //bool test = grid.Children[0].GetType() == typeof(WheelPanel);
                Assert.True(true);
            });
        }

        [Theory]
        [InlineData(0, 180)]
        [InlineData(90, 0)]
        [InlineData(-180, 90)]
        [InlineData(90, -180)]
        //
        // If the end angle is less then the start angle the end angle must equal the start angle.
        // TODO : Perhaps rethink this behavior in the future
        //
        public async Task Changing_Size_Does_Not_Change_Radius_If_Set(double outerradius, double value)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                var canvas = new Canvas();

                _unitPanel = new WheelPanelTestable(100, 100);
                _unitPanel.Width = 200;
                canvas.Children.Add(_unitPanel);
                canvas.InvalidateMeasure();
                canvas.UpdateLayout();

                Assert.True(false);
                // _unitPanel.SizeChanged.
            });
        }
#endif

        //[Theory]
        //[InlineData(100, 100, 50)]
        //[InlineData(200, 200, 100)]
        //[InlineData(100, 50, 25)]
        //[InlineData(50, 100, 25)]
        ////
        //// Width and height should set the radius unless an outerradius is defined
        ////
        //public async Task Width_Height_Sets_Actual_Radius(double width, double height, double value)
        //{
        //    await ExecuteOnUIThread<ArgumentException>(() =>
        //    {
        //        _unitPanel = new WheelPanelTestable(width, height);
        //        _unitPanel.Width = width;

        //        Assert.True(_unitPanel.ActualRadius == value);
        //    });
        //}

        //[Theory]
        //[InlineData(100, 100, 50, 50)]
        //[InlineData(200, 200, 100, 100)]
        //[InlineData(100, 50, 50, 25)]
        //[InlineData(50, 100, 25, 50)]
        ////
        //// Center should always be at the center of the width and height box.
        ////
        //public async Task Width_Height_Sets_Center(double width, double height, double centerx, double centery)
        //{
        //    await ExecuteOnUIThread<ArgumentException>(() =>
        //    {
        //        _unitPanel = new WheelPanelTestable(width, height);

        //        Assert.True((_unitPanel.Center.X == centerx) && (_unitPanel.Center.Y == centery));
        //    });
        //}
    }
}
