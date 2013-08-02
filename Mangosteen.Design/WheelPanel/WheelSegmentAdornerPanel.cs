using Microsoft.Windows.Design.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using Microsoft.Windows.Design.Model;
using System.Windows.Media;


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
            var canvas = new WheelPanelDesignTimeCanvas(_wheelPanelModelItem);

            AdornerPanel.SetAdornerHorizontalAlignment(canvas, AdornerHorizontalAlignment.Stretch);
            AdornerPanel.SetAdornerVerticalAlignment(canvas, AdornerVerticalAlignment.Stretch);

            // Placement appears to not be necessary with vertical and horizontal alignment.
            // But the adorner can be more precisely placed with the AdornerPlacementCollection class.

            Children.Add(canvas);
        }
    }
}
