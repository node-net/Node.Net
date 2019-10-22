using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net
{
    public static class PointCollectionExtension
    {
        public static Geometry GetPathGeometry(this PointCollection points)
        {
            return new PathGeometry
            {
                Figures = new PathFigureCollection{new PathFigure
                {
                    Segments = new PathSegmentCollection
                    {
                        new PolyLineSegment { Points = points }
                    }
                } }
            };
        }
    }
}
