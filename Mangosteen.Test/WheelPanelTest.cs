using Mangosteen.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Xunit;

namespace Mangosteen.Test
{
    public class WheelPanelTest
    {
        public IAsyncAction ExecuteOnUIThread<TException>(DispatchedHandler action)
            //where TException : Exception
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
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
                _unitPanel = new WheelPanel();
                _unitPanel.Width = 100;
                _unitPanel.Height = 100;
                Assert.True(_unitPanel.ActualRadius == 50);
            }); 
        }

        [Fact]
        public async Task WidthHeightSetsCenter()
        {
            await ExecuteOnUIThread<ArgumentException>(() =>
            {
                Assert.True(_unitPanel.Center.X == 50 &&
                            _unitPanel.Center.Y == 50);
            });
        }
    }
}
