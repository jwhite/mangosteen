using Microsoft.Windows.Design.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace Mangosteen.Design
{
    class WheelSegmentAdornerPanel : AdornerPanel
    {
        public WheelSegmentAdornerPanel() 
        {
            this.Loaded += WheelSegmentAdornerPanel_Loaded;
        }

        void WheelSegmentAdornerPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = new Button();
            button.Content = "This is my button";

            AdornerPanel.SetAdornerHorizontalAlignment(button, AdornerHorizontalAlignment.Stretch);
            AdornerPanel.SetAdornerVerticalAlignment(button, AdornerVerticalAlignment.Stretch);
            
            Children.Add(button);
        }
    }
}
