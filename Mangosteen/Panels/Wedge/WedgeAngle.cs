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
        public WedgeAngle(double angle) 
        {
            if (angle < 0) angle = 0;

            this._value = 0;
            this._wedgeAngleType = WedgeAngleType.Angle;
        }

        public WedgeAngle(double value, WedgeAngleType type) 
        {
            if (value < 0) value = 0;
            if (type == Wedge.WedgeAngleType.Auto) value = 0;

            this._value = value;
            this._wedgeAngleType = type;
        }

        // Comparison operators
        public static bool operator !=(WedgeAngle wa1, WedgeAngle wa2) { return !wa1.Equals(wa2); }
        public static bool operator ==(WedgeAngle wa1, WedgeAngle wa2) { return wa1.Equals(wa2); }

        // Static auto
        public static WedgeAngle Auto
        {
            get { return new WedgeAngle(0,WedgeAngleType.Auto); }
        }

        // Public properties
        public WedgeAngleType WedgeAngleType { get { return _wedgeAngleType; } }
        private WedgeAngleType _wedgeAngleType;

        public double Value { get { return _value; } }
        private double _value;

        // Tests on the wedge type
        public bool IsAngle { get { return (_wedgeAngleType == Wedge.WedgeAngleType.Angle); } }
        public bool IsAuto { get { return (_wedgeAngleType == Wedge.WedgeAngleType.Auto); } }
        public bool IsStar { get { return (_wedgeAngleType == Wedge.WedgeAngleType.Star); } }

        // From IEquality
        public bool Equals(WedgeAngle wedgeAngle) { return (_wedgeAngleType == wedgeAngle.WedgeAngleType) && (_value == wedgeAngle.Value); }
        public override bool Equals(object oCompare) 
        {
            if (oCompare is WedgeAngle)
            {
                WedgeAngle w = (WedgeAngle)oCompare;
                return (_wedgeAngleType == w.WedgeAngleType) && (_value == w.Value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode() { return this.ToString().GetHashCode(); }

        public override string ToString() 
        {
            switch (_wedgeAngleType)
            {
                case Wedge.WedgeAngleType.Angle:
                    return _value.ToString();
                                    
                case WedgeAngleType.Star:
                    return _value.ToString() + "*";
                
                case Wedge.WedgeAngleType.Auto:
                    return "Auto";
                    
                default :
                    throw new NotImplementedException("Need to return a defined string for each enum");
            } 
        }
    }
}

