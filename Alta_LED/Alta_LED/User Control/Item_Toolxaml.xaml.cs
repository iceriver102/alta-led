using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Alta_LED.Data.Init;

namespace Alta_LED.User_Control
{
    /// <summary>
    /// Interaction logic for Item_Toolxaml.xaml
    /// </summary>
    public partial class Item_Toolxaml : UserControl
    {
        //private bool _isRegularPolygon=false;
        public event EventHandler<ShapeProperty> ClickEvent;
        private  ShapeProperty _property;
        public ShapeProperty Property
        {
            get
            {
                return this._property;
            }
            set
            {
               // Type type = value.GetType();
                if (value is RegularPolygonProperty)
                {
                    RegularPolygonProperty @RegularPolygonProperty= value as RegularPolygonProperty;
                    RegularPolygon pShape = new RegularPolygon();
                    pShape.PointCount = @RegularPolygonProperty.PointCount;
                    pShape.InnerRadius= @RegularPolygonProperty.InnerRadius;
                    pShape.Width=30;
                    pShape.Height=30;
                    pShape.setPosition(17, 8);
                    this.Shape = pShape;
                }
                else if (value is ArcProperty)
                {
                    ArcProperty @ArcProperty = value as ArcProperty;
                    Arc @Arc = new Arc();
                    Arc.ArcThickness = ArcProperty.ArcThichness;
                    Arc.ArcThicknessUnit = ArcProperty.UnitType;
                    Arc.EndAngle = ArcProperty.EndAngle;
                    Arc.StartAngle = ArcProperty.StartAngle;
                    Arc.setPosition(17, 8);
                    Arc.Width = 30;
                    Arc.Height = 30;
                    this.Shape = Arc;
                }
                else if (value is BlockArrowProperty)
                {
                    BlockArrowProperty @BlockArrowProperty = value as BlockArrowProperty;
                    BlockArrow @BlockArrow = new BlockArrow();
                    BlockArrow.Orientation = BlockArrowProperty.Orientation;
                    if (BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Left || BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Right)
                    {
                        BlockArrow.Height = 16;
                        BlockArrow.Width = 30;
                        BlockArrow.setPosition(17, 15);
                    }
                    else
                    {
                        BlockArrow.Height = 30;
                        BlockArrow.Width = 16;
                        BlockArrow.setPosition(20, 8);
                    }
                    
                    this.Shape = BlockArrow;
                }
                else if (value is RectangleProperty)
                {
                    RectangleProperty @RectangleProperty = new RectangleProperty();
                    Rectangle @Rectangle = new Rectangle();
                    Rectangle.RadiusX = RectangleProperty.RadiusX;
                    Rectangle.RadiusY = RectangleProperty.RadiusY;
                    Rectangle.Width = 30;
                    Rectangle.Height = 30;
                    Rectangle.setPosition(17, 8);
                    this.Shape = Rectangle;
                }
                else if (value is EllipseProperty)
                {
                    EllipseProperty @EllipseProperty = new EllipseProperty();
                    Ellipse @Ellipse = new Ellipse();
                    Ellipse.Width = 30;
                    Ellipse.Height = 20;
                    Ellipse.setPosition(17, 11);
                    this.Shape = Ellipse;
                }
                this._property = value;

            }
        }
      //  private ShapeChilden _shapeType = ShapeChilden.Arc;
        //public ShapeChilden ShapeType
        //{
        //    get
        //    {
        //        return this._shapeType;
        //    }
        //    set
        //    {

        //        switch (value)
        //        {
        //            case ShapeChilden.Arc:
        //                Arc shape = new Arc();
        //                shape.ArcThickness = 1;
        //                shape.ArcThicknessUnit = Microsoft.Expression.Media.UnitType.Percent;
        //                shape.EndAngle = 360;
        //                shape.StartAngle = 90;
        //                shape.setPosition(17, 8);
        //                shape.Width = 30;
        //                shape.Height = 30;
        //                this.Shape = shape;
        //                break;
        //            case ShapeChilden.BlockArrow:
        //                BlockArrow tmpshape = new BlockArrow();
        //                tmpshape.Orientation = Microsoft.Expression.Media.ArrowOrientation.Left;
        //                tmpshape.Height = 16;
        //                tmpshape.Width = 30;
        //                tmpshape.setPosition(17, 15);
        //                this.Shape = tmpshape;
        //                break;
        //            case ShapeChilden.RegularPolygon:
        //                RegularPolygon tmpShape2 = new RegularPolygon();
        //                tmpShape2.PointCount = 6;
        //                tmpShape2.InnerRadius = 1;
        //                tmpShape2.setPosition(17, 8);
        //                tmpShape2.Width = 30;
        //                tmpShape2.Height = 30;
        //                this.Shape = tmpShape2;
        //                break;
        //            case ShapeChilden.Ellipse:
        //                Ellipse tmpShape3 = new Ellipse();
        //                tmpShape3.Width = 30;
        //                tmpShape3.Height = 20;
        //                tmpShape3.setPosition(17, 11);
        //                this.Shape = tmpShape3;
        //                break;
        //            case ShapeChilden.Rectangle:
        //                Rectangle tmp4Shape = new Rectangle();
        //                tmp4Shape.Width = 30;
        //                tmp4Shape.Height = 26;
        //                tmp4Shape.setPosition(17, 13);
        //                this.Shape = tmp4Shape;
        //                break;
        //            case ShapeChilden.Star:
        //                RegularPolygon tmpShapeStar = new RegularPolygon();
        //                tmpShapeStar.PointCount = 6;
        //                tmpShapeStar.InnerRadius = 0.47;
        //                tmpShapeStar.setPosition(17, 8);
        //                tmpShapeStar.Width = 30;
        //                tmpShapeStar.Height = 30;
        //                this.Shape = tmpShapeStar;
        //                break;
        //        }
        //        this._shapeType = value;
        //    }
        //}

        private Shape _shape;
        public Shape Shape
        {
            get
            {
                return this._shape;
            }

            set
            {
                this._shape = value;
                this._shape.Stroke = Brushes.Orange;
                this._shape.Fill = Brushes.White; //new SolidColorBrush(new Color() { A = 100, B = 245, G = 244, R = 244 });
                if (this._shape.Width == Double.NaN || this._shape.Width == 0)
                {
                    this._shape.Width = 30;
                }
                if (this._shape.Height == Double.NaN || this._shape.Height == 0)
                {
                    this._shape.Height = 30;
                }
                if ((double)this._shape.GetValue(Canvas.LeftProperty) == Double.NaN || (Double)this._shape.GetValue(Canvas.TopProperty) == Double.NaN)
                    this._shape.setPosition(17, 10);
                this.Canvas_Shape.Children.Clear();
                this.Canvas_Shape.Children.Add(this._shape);

            }
        }

        public String Text
        {
            get
            {
                return this.txt_content.Text;
            }
            set
            {
                this.txt_content.Text = value;
            }
        }
        public Brush TextCorlor
        {
            get
            {
                return this.txt_content.Foreground;
            }
            set
            {
                this.txt_content.Foreground = value;
            }
        }
        public Item_Toolxaml()
        {
            InitializeComponent();
           
        }

        private void Click_Event(object sender, MouseButtonEventArgs e)
        {
            if (this.ClickEvent != null)
            {
                ClickEvent(this, this.Property);
            }
        }
    }
    public enum ShapeChilden
    {
        RegularPolygon, Arc, BlockArrow, Ellipse, Rectangle
    }
}
