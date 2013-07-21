using Microsoft.Windows.Design.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using Microsoft.Windows.Design.Model;


namespace Mangosteen.Design.WheelPanel
{
    class WheelSegmentAdornerPanel : AdornerPanel
    {
        private ModelItem _wheelPanelModelItem;

        public WheelSegmentAdornerPanel(ModelItem item) 
        {
            _wheelPanelModelItem = item;
            this.Loaded += WheelSegmentAdornerPanel_Loaded;
        }

        void WheelSegmentAdornerPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            WheelPanelDesignTimeCanvas canvas = new WheelPanelDesignTimeCanvas(_wheelPanelModelItem);
            
            AdornerPanel.SetAdornerHorizontalAlignment(canvas, AdornerHorizontalAlignment.Stretch);
            AdornerPanel.SetAdornerVerticalAlignment(canvas, AdornerVerticalAlignment.Stretch);
            
            Children.Add(canvas);
        }
    }
}
