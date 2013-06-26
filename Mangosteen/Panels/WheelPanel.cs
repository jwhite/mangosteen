using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Mangosteen.Panels
{
    public sealed class WheelPanel : Panel
    {
        static WheelPanel()
        {
            
        }

        private WheelPanelDesignTimeCanvas _designTimeCanvas;

        public WheelPanel() : base()
        {
            // It is not necessary to remove the event listeners when the object goes away
            this.Loaded += WheelPanel_Loaded;
            this.SizeChanged += WheelPanel_SizeChanged;

            // Adding children here will cause them not to show up in the designer.
            // I'm not quite sure why.
                       
        }

        void WheelPanel_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDesignTimeCanvas();
        }

        /// <summary>
        /// Display that will only show up at design time to allow the user to see where the child items will be placed.
        /// </summary>
        private void CreateDesignTimeCanvas()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // Add the design time canvas
                _designTimeCanvas = new WheelPanelDesignTimeCanvas(this);
                _designTimeCanvas.CreateDesignTimeGraphic();
                _designTimeCanvas.Height = 1000;
                _designTimeCanvas.Width = 1000;
                Children.Add(_designTimeCanvas);
            }
        }

        //private void UpdatePath()
        //{
        //    //if (_isUpdating)
        //    //{
        //    //    return;
        //    //}

        //    var pathGeometry = new PathGeometry();
        //    var pathFigure = new PathFigure();
        //    pathFigure.StartPoint = new Point(Radius, Radius);
        //    pathFigure.IsClosed = true;

        //    // Starting Point
        //    var lineSegment =
        //        new LineSegment
        //        {
        //            Point = new Point(
        //                Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius,
        //                Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius)
        //        };

        //    // Arc
        //    var arcSegment = new ArcSegment();
        //    arcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
        //    arcSegment.Point =
        //        new Point(
        //                Radius + Math.Sin(EndAngle * Math.PI / 180) * Radius,
        //                Radius - Math.Cos(EndAngle * Math.PI / 180) * Radius);
        //    arcSegment.Size = new Size(Radius, Radius);
        //    arcSegment.SweepDirection = SweepDirection.Clockwise;
        //    pathFigure.Segments.Add(lineSegment);
        //    pathFigure.Segments.Add(arcSegment);
        //    pathGeometry.Figures.Add(pathFigure);
        //    //this.Data = pathGeometry;
        //    //this.InvalidateArrange();
        //}

        void WheelPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Use actual width and height to determine the radius.
            double smallestDimension = Math.Min(e.NewSize.Width, e.NewSize.Height);
            SetValue(ActualRadiusProperty, .5 * smallestDimension);
        }

        // Properties Width and Height already inherited


        //
        // Dependency properties
        //

            
        // If start and end degrees match, consider this a full 360 degree circle.
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0));

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0));

        public static readonly DependencyProperty OuterRadiusProperty =
            DependencyProperty.Register("OuterRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(Double.NaN));

        // 
        // Note : Read-Only
        //
        // Property determined by either the OuterRadius or the Height/Width after measuring.
        // If Radius is being used, then it will be non-null, otherwise only use the height and width.
        //
        public static readonly DependencyProperty ActualRadiusProperty =
            DependencyProperty.Register("ActualRadius", typeof(double), typeof(WheelPanel), new PropertyMetadata(0.0));

        

        // Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            Size s = base.MeasureOverride(availableSize);

            foreach (UIElement element in this.Children)
                element.Measure(availableSize);

            return s;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            //// Clip to ensure items dont override container
            //this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };

            //// Size and position the child elements
            //int i = 0;
            //double degreesOffset = 360.0 / this.Children.Count;

            //foreach (FrameworkElement element in this.Children)
            //{
            //    double centerX = element.DesiredSize.Width / 2.0;
            //    double centerY = element.DesiredSize.Height / 2.0;

            //    // calculate the good angle
            //    double degreesAngle = degreesOffset * i++;

            //    RotateTransform transform = new RotateTransform();
            //    transform.CenterX = centerX;
            //    transform.CenterY = centerY;
            //    // must be degrees. It's a shame it's not in radian :)
            //    transform.Angle = degreesAngle;
            //    element.RenderTransform = transform;

            //    // calculate radian angle
            //    var radianAngle = (Math.PI * degreesAngle) / 180.0;

            //    // get x and y
            //    double x = this.Radius * Math.Cos(radianAngle);
            //    double y = this.Radius * Math.Sin(radianAngle);

            //    // get real X and Y (because 0;0 is on top left and not middle of the circle)
            //    var rectX = x + (finalSize.Width / 2.0) - centerX;
            //    var rectY = y + (finalSize.Height / 2.0) - centerY;

            //    // arrange element
            //    element.Arrange(new Rect(rectX, rectY, element.DesiredSize.Width, element.DesiredSize.Height));
            //}
            return finalSize;
        }

        //
        // Back end property stores
        //
        #region Property stores

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

        #endregion

    }
}
