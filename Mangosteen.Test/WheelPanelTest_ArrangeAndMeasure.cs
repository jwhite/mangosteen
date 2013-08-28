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
using Xunit;


namespace Mangosteen.Test
{
#if false   
    public class WheelPanelTest_ArrangeAndMeasure
    {


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
    }
#endif
}
