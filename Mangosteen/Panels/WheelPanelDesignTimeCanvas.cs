using Mangosteen.ThirdParty;
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
            Children.Clear();

            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(Color.FromArgb(255, 25, 162, 222));
            ellipse.Width = 100;
            ellipse.Height = 100;
            Children.Add(ellipse);

            //Line path = new Line();
            //path.X1 = 0;
            //path.Y1 = 0;
            //path.X2 = 100;
            //path.Y2 = 100;
            //path.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            //Children.Add(path);

            Path path2 = new Path();
            GeometryGroup g = new GeometryGroup();
            EllipseGeometry e = new EllipseGeometry();
            e.Center = new Point(10,10);
            e.RadiusX = 50;
            e.RadiusY = 50;


            g.Children.Add(e);
            path2.Data = g;
            path2.Stroke = new SolidColorBrush(Color.FromArgb(255, 0,255, 0));
            Children.Add(path2);

            // Temp properties to test arcs
            var radius = 100;
            var startAngle = 0.0;
            var endAngle = 90.0;

            var center = new Point(100, 100);

            // Path geometry contains one or more elements
            // like PathFigures, Ellipses, or others...
            var pathGeometry = new PathGeometry();

            if (startAngle != endAngle)
            { 
                // Pathfigure reprents a connected series of segments
                var pathFigure = new PathFigure();

                pathFigure.StartPoint = PointFromAngleRadius(center, startAngle, radius);
                
                // Outer arc
                var outerArcSegment = new ArcSegment();
                outerArcSegment.Size = new Size(radius, radius);
                outerArcSegment.Point = PointFromAngleRadius(center, endAngle, radius);
                outerArcSegment.SweepDirection = SweepDirection.Clockwise;

                pathFigure.Segments.Add(outerArcSegment);

                pathGeometry.Figures.Add(pathFigure);

            } else {
                // Draw two ellipse

            }

            var path = new Path();
            path.Fill = new SolidColorBrush(Colors.White);
            path.Stroke = new SolidColorBrush(Colors.Red);
            path.Data = pathGeometry;

            //Children.Add(pathGeometry);


            Children.Add(path);
        }

        public static Point PointFromAngleRadius(Point center, double angle, double radius)
        {
            return new Point(center.X + Math.Sin(angle * Math.PI / 180.0) * radius,
                             center.Y + Math.Cos(angle * Math.PI / 180.0) * radius);
        }
    }
}

