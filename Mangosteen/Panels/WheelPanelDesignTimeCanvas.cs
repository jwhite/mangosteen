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
    public class WheelPanelDesignTimeCanvas : Canvas
    {
        private WheelPanel _panel;
        public WheelPanelDesignTimeCanvas(WheelPanel parentPanel)
        {
            _panel = parentPanel;
        }

        public void CreateDesignTimeGraphic()
        {
            if (_panel == null) throw new ArgumentNullException();

            Children.Clear();

            // Temp properties to test arcs
            var outerRadius = _panel.ActualRadius;
            var innerRadius = _panel.InnerRadius;
            var startAngle = _panel.StartAngle;
            var endAngle = _panel.EndAngle;
            var center = _panel.Center;

            var path = new Path();

            path.Stroke = new SolidColorBrush(Colors.Red);

            if (startAngle != endAngle)
            {
                var pathGeometry = new PathGeometry();

                // Pathfigure reprents a connected series of segments
                var pathFigure = new PathFigure();
                pathFigure.IsClosed = true;
                pathFigure.IsFilled = false;

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
                    Point = PointFromAngleRadius(center, endAngle, innerRadius == Double.NaN ? 0 : innerRadius)
                };
                pathFigure.Segments.Add(lineSegment);

                // Don't draw the inner arc if there is no inner radius, draw a pie-shaped slice
                if (innerRadius != Double.NaN)
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

            } else {
                // Draw two ellipse
                var group = new GeometryGroup();

                EllipseGeometry outerEllipse = new EllipseGeometry();
                outerEllipse.Center = center;
                outerEllipse.RadiusX = outerRadius;
                outerEllipse.RadiusY = outerRadius;

                group.Children.Add(outerEllipse);

                if (innerRadius != Double.NaN)
                {
                    EllipseGeometry innerEllipse = new EllipseGeometry();
                    innerEllipse.Center = center;
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

