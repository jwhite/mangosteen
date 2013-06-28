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

namespace Mangosteen.Test
{
    public class WheelPanelTestable : WheelPanel
    {
        public WheelPanelTestable(double width, double height) : base()
        {
            Width = width;
            Height = height;
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

        [Fact]
        public async Task WidthHeightSetsRadius()
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(100,100);
                Assert.True(_unitPanel.ActualRadius == 50);
            }); 
        }

        [Fact]
        public async Task WidthHeightSetsCenter()
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                _unitPanel = new WheelPanelTestable(100, 100);
                Assert.True(_unitPanel.Center.X == 50 &&
                            _unitPanel.Center.Y == 50);
            });
        }
    }
}
