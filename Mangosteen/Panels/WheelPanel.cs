using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Mangosteen.Panels;
using System.Collections.ObjectModel;
using Mangosteen.Panels.Wedge;

namespace Mangosteen.Panels
{
    public class WheelPanel : Panel
    {
        public WheelPanel()
        {
            // It is not necessary to remove the event listeners when the object goes away
            this.SizeChanged += WheelPanel_SizeChanged;

            // Setting the bindings here fails in a non-deterministic manner during the unit test 
#if false
            Binding bWidth = new Binding() { Path = new PropertyPath("Width"), Source = this };
            var widthExProp = DependencyProperty.RegisterAttached("Width",
                                typeof(object),
                                typeof(UserControl),
                                new PropertyMetadata(null, OnWidthChanged));
            SetBinding(widthExProp, bWidth);

            Binding bHeight = new Binding() { Path = new PropertyPath("Height"), Source = this };
            var heightExProp = DependencyProperty.RegisterAttached("Height",
                                typeof(object),
                                typeof(UserControl),
                                new PropertyMetadata(null, OnHeightChanged));
            SetBinding(heightExProp, bHeight);
#endif
        }


        void WheelPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetValue(CenterProperty, CalculateCenter(e.NewSize.Width,e.NewSize.Height));

            if (Double.IsNaN(OuterRadius))
            {
                // Use actual width and height to determine the radius.
                double smallestDimension = Math.Min(e.NewSize.Width, e.NewSize.Height);
                SetValue(ActualRadiusProperty, .5 * smallestDimension);
            }
        }

        //
        // Dependency properties
        //
                    
        // If start and end degrees match, consider this a full 360 degree circle.
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0, OnStartAngleChanged));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0, OnEndAngleChanged));

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(Double.NaN, OnInnerRadiusChanged));

        public static readonly DependencyProperty OuterRadiusProperty =
            DependencyProperty.Register("OuterRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(Double.NaN, OnOuterRadiusChanged));

        // Using a DependencyProperty as the backing store for WedgeDefinitions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WedgeDefinitionsProperty =
            DependencyProperty.Register("WedgeDefinitions", typeof(WedgeDefinitionCollection), typeof(WheelPanel), new PropertyMetadata(null));

        
        // 
        // Note : Read-Only
        //
        // Property determined by either the OuterRadius or the Height/Width after measuring.
        // If Radius is being used, then it will be non-null, otherwise only use the height and width.
        //
        public static readonly DependencyProperty ActualRadiusProperty =
            DependencyProperty.Register("ActualRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(Double.NaN, OnActualRadiusChanged));

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(WheelPanel), new PropertyMetadata(null));


        private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue > (d as WheelPanel).EndAngle)
            {
                (d as WheelPanel).EndAngle = (double)e.NewValue;
            }
        }

        private static void OnEndAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as WheelPanel).StartAngle > (double)e.NewValue)
            {
                (d as WheelPanel).StartAngle = (double)e.NewValue;
            }
        }

        private static void OnOuterRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // If it is defined set the actual radius directly
            if (!Double.IsNaN((double)e.NewValue))
            {
                (d as WheelPanel).SetValue(ActualRadiusProperty, (double)e.NewValue);
            }
            else 
            { 
                // Width and height should set it
                (d as WheelPanel).SetValue(ActualRadiusProperty, CalculateOuterRadiusFromWidthHeight((d as WheelPanel).Width, (d as WheelPanel).Height));
            }
        }

        private static void OnInnerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Don't let the inner radius be negative
            if ((double)e.NewValue < 0)
            {
                (d as WheelPanel).InnerRadius = 0;
            }
            else
            {
                // Don't let the inner radius be greater then the outer radius
                if ((double)e.NewValue > (d as WheelPanel).ActualRadius)
                {
                    (d as WheelPanel).InnerRadius = (d as WheelPanel).ActualRadius;
                }
            }
        }

        private static void OnActualRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Coerce the InnerRadius
            if ((d as WheelPanel).InnerRadius > (double)e.NewValue)
            {
                (d as WheelPanel).InnerRadius = (double)e.NewValue;
            }
        }

        private static double CalculateOuterRadiusFromWidthHeight(double width, double height)
        {
            return (Math.Min(width, height) / 2.0);
        }

        private static Point CalculateCenter(double width, double height)
        {
            return new Point(width / 2.0, height / 2.0);
        }

        // Overrides

        //
        // During the measure/arrange pass the WedgeAngleCalculations need to be populated.
        // 
        // First, the table needs to be built with the number of elements equal to the smaller of :
        //    A: The total number of WedgeAngle definitions or 
        //    B: The number of child elements 
        // 
        // Then, if the number of child elements is greater then the number of wege defintions the wedge
        // definitions are assumed to repeat to cover the whole WedgeAngleCalculations table.
        //
        // Next the elements associated with a fixed angle size should be populated with their calculation values.
        // 
        // Then, the elements with size 'auto' should be populated based on the size of the child controls.
        // 
        // Lastly, the remaining space should be divided up among the star sized controls.

        /// <summary>
        /// Measures each of the children and reports how much size the panel will require.
        /// 
        /// Then, uses the child measurements to build a table for the arrange pass.
        /// </summary>
        /// <param name="availableSize">The total amount of space available for the wrap panel.</param>
        /// <returns>The space required by the panel and all of its child elements</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size s = base.MeasureOverride(availableSize);

            // do something with child.DesiredSize, either sum them directly or apply whatever logic your element has for reinterpreting the child sizes 
            // if greater than availableSize, must decide what to do and which size to return
            foreach (UIElement element in this.Children)
                element.Measure(availableSize);

            // desiredSize = ... computed sum of children's DesiredSize ...; 
            // IMPORTANT: do not allow PositiveInfinity to be returned, that will raise an exception in the caller! 
            // PositiveInfinity might be an availableSize input; this means that the parent does not care about sizing 
            return s;
        }

        //protected override Size ArrangeOverride(Size finalSize)
        //{

        //    //// Clip to ensure items dont override container
        //    //this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };

        //    //// Size and position the child elements
        //    //int i = 0;
        //    //double degreesOffset = 360.0 / this.Children.Count;

        //    //foreach (FrameworkElement element in this.Children)
        //    //{
        //    //    double centerX = element.DesiredSize.Width / 2.0;
        //    //    double centerY = element.DesiredSize.Height / 2.0;

        //    //    // calculate the good angle
        //    //    double degreesAngle = degreesOffset * i++;

        //    //    RotateTransform transform = new RotateTransform();
        //    //    transform.CenterX = centerX;
        //    //    transform.CenterY = centerY;
        //    //    // must be degrees. It's a shame it's not in radian :)
        //    //    transform.Angle = degreesAngle;
        //    //    element.RenderTransform = transform;

        //    //    // calculate radian angle
        //    //    var radianAngle = (Math.PI * degreesAngle) / 180.0;

        //    //    // get x and y
        //    //    double x = this.Radius * Math.Cos(radianAngle);
        //    //    double y = this.Radius * Math.Sin(radianAngle);

        //    //    // get real X and Y (because 0;0 is on top left and not middle of the circle)
        //    //    var rectX = x + (finalSize.Width / 2.0) - centerX;
        //    //    var rectY = y + (finalSize.Height / 2.0) - centerY;

        //    //    // arrange element
        //    //    element.Arrange(new Rect(rectX, rectY, element.DesiredSize.Width, element.DesiredSize.Height));
        //    //}
        //    return finalSize;
        //}

        //
        // Back end property stores
        //

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }
        #region Property stores

        // Read-only
        public double ActualRadius
        {
            get { return (double)GetValue(ActualRadiusProperty); }
            //set { SetValue(RadiusProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            //set { SetValue(CenterProperty, value); }
        }

        public WedgeDefinitionCollection WedgeDefinitions
        {
            get { return (WedgeDefinitionCollection)GetValue(WedgeDefinitionsProperty); }
            set { SetValue(WedgeDefinitionsProperty, value); }
        }

        

        #endregion
    }
}
