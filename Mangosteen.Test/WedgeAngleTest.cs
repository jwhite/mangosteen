using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Mangosteen.Panels;
using Mangosteen.Panels.Wedge;

namespace Mangosteen.Test
{
    public class WedgeAngleTest
    {
        [Fact]
        public void WedgeAngle_Can_Be_Constructed()
        {
            WedgeAngle wa = new WedgeAngle();

            Assert.True(wa.Value == 0);
            Assert.True(wa.WedgeAngleType == WedgeAngleType.Auto);
        }

        [Theory]
        [InlineData(0, WedgeAngleType.Auto)]
        [InlineData(90, WedgeAngleType.Star)]
        [InlineData(180, WedgeAngleType.Angle)]
        public void WedgeAngle_Can_Be_Constructed_Parameters(double value, WedgeAngleType type)
        {
            WedgeAngle wa = new WedgeAngle(value, type);

            Assert.True((value == wa.Value) &&
                         (type == wa.WedgeAngleType));
        }

        [Fact]
        public void WedgeAngle_Value_Cannot_Be_Negative()
        {
            WedgeAngle wa = new WedgeAngle(-180, WedgeAngleType.Angle);

            Assert.True(wa.Value == 0);
        }

        [Fact]
        public void WedgeAngle_Auto_Cannot_Have_Value()
        {
            WedgeAngle wa = new WedgeAngle(90, WedgeAngleType.Auto);

            Assert.True(wa.Value == 0);
        }

        [Theory]
        [InlineData(0, WedgeAngleType.Auto, 0, WedgeAngleType.Auto, false)]
        [InlineData(90, WedgeAngleType.Star, 90, WedgeAngleType.Angle, true)]
        [InlineData(90, WedgeAngleType.Star, 30, WedgeAngleType.Star, true)]
        [InlineData(90, WedgeAngleType.Star, 90, WedgeAngleType.Star, false)]
        [InlineData(20, WedgeAngleType.Angle, 20, WedgeAngleType.Angle, false)]
        [InlineData(0, WedgeAngleType.Auto, 90, WedgeAngleType.Auto, false)]
        public void WedgeAngle_Inequality(double va1, WedgeAngleType t1, double va2, WedgeAngleType t2, bool expected)
        {
            WedgeAngle wa1 = new WedgeAngle(va1, t1);
            WedgeAngle wa2 = new WedgeAngle(va2, t2);

            bool b = (wa1 != wa2);

            Assert.True(b == expected);
        }

        [Theory]
        [InlineData(0, WedgeAngleType.Auto, 0, WedgeAngleType.Auto, true)]
        [InlineData(90, WedgeAngleType.Star, 90, WedgeAngleType.Angle, false)]
        [InlineData(90, WedgeAngleType.Star, 30, WedgeAngleType.Star, false)]
        [InlineData(90, WedgeAngleType.Star, 90, WedgeAngleType.Star, true)]
        [InlineData(20, WedgeAngleType.Angle, 20, WedgeAngleType.Angle, true)]
        [InlineData(0, WedgeAngleType.Auto, 90, WedgeAngleType.Auto, true)]
        public void WedgeAngle_Equality(double va1, WedgeAngleType t1, double va2, WedgeAngleType t2, bool expected)
        {
            WedgeAngle wa1 = new WedgeAngle(va1, t1);
            WedgeAngle wa2 = new WedgeAngle(va2, t2);

            bool b = (wa1 == wa2);

            Assert.True(b == expected);
        }

        [Fact]
        public void WedgeAngle_Static_Auto()
        {
            WedgeAngle s = WedgeAngle.Auto;

            Assert.True(s.Value == 0);
            Assert.True(s.WedgeAngleType == WedgeAngleType.Auto);
        }

        [Theory]
        [InlineData(WedgeAngleType.Angle, true)]
        [InlineData(WedgeAngleType.Star, false)]
        [InlineData(WedgeAngleType.Auto, false)]
        public void WedgeAngle_Is_Angle(WedgeAngleType type, bool expected)
        {
            WedgeAngle wa = new WedgeAngle(0, type);
            Assert.True(wa.IsAngle == expected);
        }

        [Theory]
        [InlineData(WedgeAngleType.Angle, false)]
        [InlineData(WedgeAngleType.Star, true)]
        [InlineData(WedgeAngleType.Auto, false)]
        public void WedgeAngle_Is_Star(WedgeAngleType type, bool expected)
        {
            WedgeAngle wa = new WedgeAngle(0, type);
            Assert.True(wa.IsStar == expected);
        }

        [Theory]
        [InlineData(WedgeAngleType.Angle, false)]
        [InlineData(WedgeAngleType.Star, false)]
        [InlineData(WedgeAngleType.Auto, true)]
        public void WedgeAngle_Is_Auto(WedgeAngleType type, bool expected)
        {
            WedgeAngle wa = new WedgeAngle(0, type);
            Assert.True(wa.IsAuto == expected);
        }

        [Fact]
        public void WedgeAngle_Equals()
        {
            WedgeAngle wa = new WedgeAngle(0, WedgeAngleType.Angle);
            WedgeAngle wa2 = new WedgeAngle(0, WedgeAngleType.Angle);
            WedgeAngle wa3 = new WedgeAngle(90, WedgeAngleType.Angle);
            WedgeAngle wa4 = new WedgeAngle(0, WedgeAngleType.Star);
            Assert.True(wa.Equals(wa2));
            Assert.False(wa.Equals(wa3));
            Assert.False(wa.Equals(wa4));
        }

        [Fact]
        public void WedgeAngle_Equals_Object()
        {
            WedgeAngle wa = new WedgeAngle(0, WedgeAngleType.Angle);
            WedgeAngle wa2 = new WedgeAngle(0, WedgeAngleType.Angle);
            Assert.False(wa.Equals(null));
            Assert.True(wa.Equals((object)wa2));
        }

        [Fact]
        public void WedgeAngle_HashCode()
        {
            WedgeAngle wa = new WedgeAngle(0, WedgeAngleType.Angle);
            WedgeAngle wa2 = new WedgeAngle(0, WedgeAngleType.Angle);
            WedgeAngle wa3 = new WedgeAngle(90, WedgeAngleType.Angle);

            Assert.True(wa.GetHashCode() == wa2.GetHashCode());
            Assert.False(wa.GetHashCode() == wa3.GetHashCode());
        }

        [Theory]
        [InlineData(0, WedgeAngleType.Auto, "Auto")]
        [InlineData(90, WedgeAngleType.Star, "90*")]
        [InlineData(180, WedgeAngleType.Angle, "180")]
        public void WedgeAngle_ToString(double value, WedgeAngleType type, string expected)
        {
            WedgeAngle wa = new WedgeAngle(value, type);

            Assert.True(wa.ToString() == expected);
        }
    }
}
