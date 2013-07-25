using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Mangosteen.Panels.Wedge
{
    //
    // Bindable defintion of a single wedge.
    // Similar to column definition in WPF.
    //
    public class WedgeDefinition : DependencyObject
    {
        public WedgeDefinition() {}

        //
        // When columns are added and removed, ActualAngle becomes 0 until measure is called.
        //
        public static readonly DependencyProperty ActualAngleProperty = DependencyProperty.Register("ActualAngle",
                                typeof(double),
                                typeof(WedgeDefinition),
                                new PropertyMetadata(0.0));
        // Read Only
        public double ActualAngle 
        { 
            get { return (double)GetValue(ActualAngleProperty); } 
            // set
        }

        // Summary:
        //    //     Gets or sets a value that represents the maximum width of a ColumnDefinition.
        //    //
        //    // Returns:
        //    //     A Double that represents the maximum width in pixels. The default is PositiveInfinity.
        public double MaxAngle 
        { 
            get {return (double)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }

        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", 
                                        typeof(double), 
                                        typeof(WedgeDefinition), 
                                        new PropertyMetadata(double.PositiveInfinity));

        public double MinAngle
        {
            get { return (double)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }
        public static DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", 
                                        typeof(double), 
                                        typeof(WedgeDefinition), 
                                        new PropertyMetadata(0.0));

        //    //     The GridLength that represents the width of the column. The default value
        //    //     is 1.0.
        public WedgeAngle Angle 
        {
            get { return (WedgeAngle)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", 
                                        typeof(double), 
                                        typeof(WedgeDefinition), 
                                        new PropertyMetadata(new WedgeAngle(1.0, WedgeAngleType.Star)));
    }
}

