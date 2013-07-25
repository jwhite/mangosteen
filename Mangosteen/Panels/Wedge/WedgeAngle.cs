using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangosteen.Panels.Wedge
{
    public enum WedgeAngleType 
    {   
        //The size is determined by the size properties of the content object.
        Auto = 0,	

        //The value is expressed in degrees
        Angle = 1,

        //The value is expressed as a weighted proportion of available space.
        Star = 2,
    }

    // This class is similar to GridLength used in the Grid control
    // That class is defined in Windows.UI.Xaml
    public struct WedgeAngle
    {
        public WedgeAngle(double angle) {}
        public WedgeAngle(double value, WedgeAngleType type) { int i = 0; }

        // Comparison operators
        public static bool operator !=(WedgeAngle wa1, WedgeAngle wa2) { return true; }
        public static bool operator ==(WedgeAngle wa1, WedgeAngle wa2) { return true; }

        public static WedgeAngle Auto
        {
            get { return new WedgeAngle(); }
        }

        public WedgeAngleType WedgeAngleType { get { return WedgeAngleType.Auto; } }
        public double Value { get { return Double.NaN; } }

        // Tests on the wedge type
        public bool IsAbsolute { get { return false; } }
        public bool IsAuto { get { return false; }}
        public bool IsStar { get { return false; }}

        public bool Equals(WedgeAngle wedgeAngle) { return true; }
        public override bool Equals(object oCompare) { return true; }

        public override int GetHashCode() { return 0; }

        public override string ToString() { return ""; }
    }
}

