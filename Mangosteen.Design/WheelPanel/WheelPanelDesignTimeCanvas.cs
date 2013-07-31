//
// Redefine Win8ControlLibrary to be the namespace of our external WinRT dll.
//
///extern alias MetroControlLibrary;
//using Win8ControlLibrary = MetroControlLibrary::Mangosteen.Panels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Microsoft.Windows.Design.Model;
using WinRTPoint = Windows.Foundation.Point;


namespace Mangosteen.Design.WheelPanel
{
    public class WheelPanelDesignTimeCanvas : Canvas
    {
        private ModelItem _item;

        public WheelPanelDesignTimeCanvas(ModelItem item)
        {
            _item = item;

            this.Loaded += WheelSegmentAdornerPanel_Loaded;
            this.SizeChanged += WheelPanelDesignTimeCanvas_SizeChanged;

            item.PropertyChanged += Item_PropertyChanged;
        }

        void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateDesignTimeGraphic();
        }

        private void WheelSegmentAdornerPanel_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDesignTimeGraphic();
        }

        void WheelPanelDesignTimeCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDesignTimeGraphic();
        }

        public void UpdateDesignTimeGraphic()
        {
            if (_item == null) throw new ArgumentNullException();

            Children.Clear();

            // Temp properties to test arcs
            double outerRadius = (double)_item.Properties["ActualRadius"].ComputedValue;
            double innerRadius = (double)_item.Properties["InnerRadius"].ComputedValue;
            double startAngle =  (double)_item.Properties["StartAngle"].ComputedValue;
            double endAngle =    (double)_item.Properties["EndAngle"].ComputedValue;
            WinRTPoint rt_center = (WinRTPoint)_item.Properties["Center"].ComputedValue;

            Point center = DesignerExtentionUtilities.ConvertFromWinRTPoint(rt_center);

            var path = new Path();

            path.Stroke = new SolidColorBrush(Colors.Red);
            path.StrokeDashArray = new DoubleCollection(new List<double> { 3, 2 });
            path.StrokeThickness = 1.0;
            

            if (startAngle != endAngle)
            {
                var pathGeometry = new PathGeometry();

                // Pathfigure reprents a connected series of segments
                var pathFigure = new PathFigure();
                pathFigure.IsClosed = true;
                pathFigure.IsFilled = false;

                // Start at the start angle on the outside
                pathFigure.StartPoint = PointFromAngleRadius(center, startAngle, outerRadius);

                // Outer arc
                var outerArcSegment = new ArcSegment();
                outerArcSegment.Size = new Size(outerRadius, outerRadius);
                outerArcSegment.Point = PointFromAngleRadius(center, endAngle, outerRadius);
                outerArcSegment.SweepDirection = SweepDirection.Clockwise;
                outerArcSegment.IsLargeArc = (endAngle - startAngle) >= 180;

                pathFigure.Segments.Add(outerArcSegment);

                // Line to connect the outer segment to the inner segment
                var lineSegment = new LineSegment()
                {
                    Point = PointFromAngleRadius(center, endAngle, Double.IsNaN(innerRadius) ? 0 : innerRadius)
                };
                pathFigure.Segments.Add(lineSegment);

                // Don't draw the inner arc if there is no inner radius, draw a pie-shaped slice
                if (!Double.IsNaN(innerRadius))
                {
                    // Inner arc
                    var innerArcSegment = new ArcSegment();
                    innerArcSegment.Size = new Size(innerRadius, innerRadius);
                    innerArcSegment.Point = PointFromAngleRadius(center, startAngle, innerRadius);
                    innerArcSegment.SweepDirection = SweepDirection.Counterclockwise;
                    innerArcSegment.IsLargeArc = (endAngle - startAngle) >= 180;

                    pathFigure.Segments.Add(innerArcSegment);
                }

                pathGeometry.Figures.Add(pathFigure);
                path.Data = pathGeometry;
            }
            else
            {
                // Draw two ellipses
                var group = new GeometryGroup();

                EllipseGeometry outerEllipse = new EllipseGeometry();
                outerEllipse.Center = new Point(center.X, center.Y);
                outerEllipse.RadiusX = outerRadius;
                outerEllipse.RadiusY = outerRadius;

                group.Children.Add(outerEllipse);

                if (!Double.IsNaN(innerRadius))
                {
                    EllipseGeometry innerEllipse = new EllipseGeometry();
                    innerEllipse.Center = new Point(center.X, center.Y);
                    innerEllipse.RadiusX = innerRadius;
                    innerEllipse.RadiusY = innerRadius;

                    group.Children.Add(innerEllipse);
                }

                path.Data = group;
            }

            Children.Add(path);
        }

        public static Point PointFromAngleRadius(Point center, double angle, double radius)
        {
            return new Point(center.X + Math.Sin(angle * Math.PI / 180.0) * radius,
                             center.Y - Math.Cos(angle * Math.PI / 180.0) * radius);
        }
    }
}
