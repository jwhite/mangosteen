using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangosteen.Design
{
    public class DesignerExtentionUtilities
    {
        public static System.Windows.Point ConvertFromWinRTPoint(Windows.Foundation.Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}
