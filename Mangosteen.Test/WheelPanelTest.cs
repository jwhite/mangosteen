using Mangosteen.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mangosteen.Test
{
    public class WheelPanelTest
    {
        WheelPanel _unitPanel;

        public WheelPanelTest()
        {
            _unitPanel = new WheelPanel();
        }

        [Fact]
        public void WheelPanel_Can_Be_Constructed()
        {
            Assert.True(_unitPanel != null);
        }

        [Fact]
        public void WidthHeightSetsRadius()
        {
            _unitPanel.Width = 100;
            _unitPanel.Height = 100;

            Assert.True(_unitPanel.ActualRadius == 50);
        }

        [Fact]
        public void WidthHeightSetsCenter()
        {
            Assert.True(_unitPanel.Center.X == 50 &&
                        _unitPanel.Center.Y == 50);
        }
    }
}
