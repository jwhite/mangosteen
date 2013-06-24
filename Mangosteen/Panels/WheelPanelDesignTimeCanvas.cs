using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Mangosteen.Panels
{
    public class WheelPanelDesignTimeCanvas : Canvas
    {
        private WheelPanel _panel;
        public WheelPanelDesignTimeCanvas(WheelPanel parentPanel)
        {
            _panel = parentPanel;
        }

        public void CreateDesignTimeGraphic()
        {
            Children.Clear();

            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 25, 162, 222));
            ellipse.Width = 100;
            ellipse.Height = 100;
            Children.Add(ellipse);
        }
    }
}
