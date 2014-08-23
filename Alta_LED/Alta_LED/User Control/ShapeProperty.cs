using Microsoft.Expression.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Alta_LED.User_Control
{
    [XmlInclude(typeof(RectangleProperty))]
    [XmlInclude(typeof(RegularPolygonProperty))]
    [XmlInclude(typeof(ArcProperty))]
    [XmlInclude(typeof(BlockArrowProperty))]
    [XmlInclude(typeof(EllipseProperty))]
    [Serializable]
    public abstract class ShapeProperty
    {
        public ShapeChilden Type;
       
        public double Width { get; set; }
        public double Height { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double FixTop
        {
            get;
            private set;
        }
        
        public double FixLeft
        {
            get;
            private set;
        }

        public ShapeProperty()
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = 100;
            this.Height = 100;            
        }
        public ShapeProperty(ShapeChilden type)
        {
            this.Type = type;
            this.Width = 100;
            this.Height = 100;
        }
        public void setFixCode()
        {
            this.FixLeft = this.Left;
            this.FixTop = this.Top;
        }
    }
    [Serializable]
    public class RegularPolygonProperty : ShapeProperty
    {
        public RegularPolygonProperty():base()
        {
            this.Type = ShapeChilden.RegularPolygon;
        }
        public RegularPolygonProperty(ShapeChilden type):base(type)
        {
        }

        public double InnerRadius { get; set; }
        public int PointCount { get; set; }

    }
    [Serializable]
    public class ArcProperty: ShapeProperty
    {
        public ArcProperty():base()
        {
            this.Type = ShapeChilden.Arc;
        }
        public UnitType @UnitType { get; set; }
        public double ArcThichness { get; set; }
        public double EndAngle { get; set; }
        public double StartAngle { get; set; }

    }
    [Serializable]
    public class BlockArrowProperty : ShapeProperty
    {
        public BlockArrowProperty():base()
        {
            this.Type = ShapeChilden.BlockArrow;
        }
        public ArrowOrientation Orientation { get; set; }

    }
    [Serializable]
    public class EllipseProperty : ShapeProperty
    {
        public EllipseProperty():base()
        {
            this.Type = ShapeChilden.Ellipse;
        }

    }
    [Serializable]
    public class RectangleProperty : ShapeProperty
    {
        public RectangleProperty():base()
        {
            this.Type = ShapeChilden.Rectangle;
        }
        public double RadiusX
        {
            get;
            set;
        }
        public double RadiusY
        {
            get;
            set;
        }
    }


}
