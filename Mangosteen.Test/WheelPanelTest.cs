using Mangosteen.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Xunit;
using Xunit.Extensions;

namespace Mangosteen.Test
{
    public class WheelPanelTestable : WheelPanel
    {
        private double radius;

        public WheelPanelTestable(double width, double height) : base()
        {
            Width = width;
            Height = height;
        }

        public WheelPanelTestable(double radius)
        {
            // TODO: Complete member initialization
            this.radius = radius;
        }
    }

    public class WheelPanelTest
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        WheelPanel _unitPanel;

        [Fact]
        public async void WheelPanel_Can_Be_Constructed()
        {
            await ExecuteOnUIThread<ArgumentException>(() => _unitPanel = new WheelPanel());
            Assert.True(_unitPanel != null);
        }

        [Theory]
        [InlineData(100, 100, 50)]
        [InlineData(200, 200, 100)]
        [InlineData(100, 50, 25)]
        [InlineData(50, 100, 25)]
        //
        // Width and height should set the radius unless an outerradius is defined
        //
        public async Task WidthHeightSetsActualRadius(double width, double height, double value)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(width, height);
                _unitPanel.Width = width;

                Assert.True(_unitPanel.ActualRadius == value);
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
        public async Task WidthHeightSetsCenter(double width, double height, double centerx, double centery)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(width, height);
                Assert.True((_unitPanel.Center.X == centerx) && (_unitPanel.Center.Y == centery));
            });
        }

        [Theory]
        [InlineData(50, 50, 50, 50)]
        [InlineData(20, 50, 50, 20)]
        //
        // The read only property of ActualRadius should be set by the radius (if set) regardless of the 
        // Width and height.
        //
        public async Task RadiusSetsActualRadius(double radius, double width, double height, double value)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(width, height);
                _unitPanel.Width = width;
                _unitPanel.OuterRadius = radius;
                Assert.True(_unitPanel.ActualRadius == value);
            });
        }

        [Theory]
        [InlineData(25, 25)]
        [InlineData(75, 50)]
        //
        // The inner raduis can be 0, but should be bounded by the outer radius.  (It can be no bigger.)
        //
        public async Task InnerRadiusIsBoundedByOuterRadius(double innerradius, double value)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(100, 100);
                _unitPanel.OuterRadius = 50;
                _unitPanel.InnerRadius = innerradius;
                Assert.True(_unitPanel.InnerRadius == value);
            });
        }

        [Theory]
        [InlineData(0, 180,180)]
        [InlineData(90, 0, 0)]
        [InlineData(-180,90,90)]
        [InlineData(90,-180,90)]
        //
        // If the end angle is less then the start angle the end angle must equal the start angle.
        // TODO : Perhaps rethink this behavior in the future
        //
        public async Task EndAngleMustBeGreaterThenStartAngle(double startangle, double endangle, double value)
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(100, 100);
                _unitPanel.StartAngle = startangle;
                _unitPanel.EndAngle = endangle;
                Assert.True(_unitPanel.EndAngle == value);
            });
        }
    }
}
