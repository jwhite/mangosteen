using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Mangosteen.Panels.Wedge
{
    public class WedgeAngleCalculation
    {
        public WedgeAngleCalculation(WedgeAngle wa, FrameworkElement e)
        {
            WedgeAngle = wa;
            Element = e;
        }

        public double CalculatedWedgeAngle;

        public WedgeAngle WedgeAngle;
        public FrameworkElement Element;
    }
}
