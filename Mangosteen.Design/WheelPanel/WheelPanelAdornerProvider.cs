extern alias MetroControlLibrary;
using Win8ControlLibrary = MetroControlLibrary::Mangosteen.Panels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Model;
using System.Diagnostics;

namespace Mangosteen.Design.WheelPanel
{
    /// <summary>
    /// The following class implements an adorner provider for WheelPanel
    /// It will draw the segments defined by the attached child properties 
    /// similarly to way the grid control defines rows and columns.
    /// </summary>
    class WheelPanelAdornerProvider : PrimarySelectionAdornerProvider
    {
        private AdornerPanel wheelSegmentsPanel;

        public WheelPanelAdornerProvider() { }

        protected override void Activate(ModelItem item)
        {
            //Debugger.Launch();

            // Create and add a new adorner panel
            wheelSegmentsPanel = new WheelSegmentAdornerPanel(item);
            Adorners.Add(wheelSegmentsPanel);

            base.Activate(item);
        }

        protected override void Deactivate()
        {
            base.Deactivate();
        }
    }
}
