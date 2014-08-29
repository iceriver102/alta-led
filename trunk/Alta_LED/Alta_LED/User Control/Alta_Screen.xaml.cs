using Alta_LED.Data.Init;
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
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace Alta_LED.User_Control
{
    /// <summary>
    /// Interaction logic for alta_Screen.xaml
    /// </summary>
    public partial class alta_Screen : UserControl
    {
        private double x, y;
        private double x2, y2;
        public double maxLeft
        {
            get
            {
                return x2;

            }
            set
            {
                x2 = value;
                this.RealWidth = value - this.minLeft;
            }
        }
        public double maxTop
        {
            get
            {
                return y2;
            }
            set
            {
                y2 = value;
                this.RealHeight = value - this.minTop;
            }
        }
        public double minLeft
        {
            get { return x; }

            set
            {
                x = value;
                this.RealWidth = this.maxLeft - value;
                int count = this.Properties.Count;
                for (int i = 0; i < count; i++)
                {
                    this.Properties[i].Left = this.Properties[i].FixLeft - value;

                }
            }
        }
        public double minTop
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                this.RealHeight = this.maxTop - value;
                int count = this.Properties.Count;
                for (int i = 0; i < count; i++)
                {
                    this.Properties[i].Top = this.Properties[i].FixTop - value;
                }
            }
        }
        public double Left
        {
            get
            {
                return this.getPosition().X;
            }
            set
            {
                this.setPosition(value, Double.NaN);
            }
        }

        public double Top
        {
            get
            {
                return this.getPosition().Y;
            }
            set
            {
                this.setPosition(Double.NaN, value);
            }

        }
        public double fixWidth
        {
            get;
            private set;
        }
        public double fixHeight
        {
            get;
            private set;
        }
        public List<Shape> Children
        {
            get;
            private set;
        }
        public List<ShapeProperty> Properties
        {
            get;
            private set;
        }
        private int _zindex = 100;
        public int zIndex
        {
            get
            {
                return this._zindex;
            }
            set
            {
                this._zindex = value;
                Canvas.SetZIndex(this, value);
            }
        }
        private bool _run = false;
        private Brush tmpBrush;
        public bool isRunning
        {
            get
            {
                return _run;
            }
            set
            {
                _run = value;

                if (value)
                {
                    this.tmpBrush = this.OutLine.BorderBrush;
                    this.OutLine.BorderBrush = Brushes.Transparent;
                    this.rect_00.Visibility = Visibility.Hidden;
                    this.rect_01.Visibility = Visibility.Hidden;
                    this.rect_02.Visibility = Visibility.Hidden;
                    this.rect_10.Visibility = Visibility.Hidden;
                    this.rect_12.Visibility = Visibility.Hidden;
                    this.rect_20.Visibility = Visibility.Hidden;
                    this.rect_21.Visibility = Visibility.Hidden;
                    this.rect_22.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.OutLine.BorderBrush = this.tmpBrush;
                    this.rect_00.Visibility = Visibility.Visible;
                    this.rect_01.Visibility = Visibility.Visible;
                    this.rect_02.Visibility = Visibility.Visible;
                    this.rect_10.Visibility = Visibility.Visible;
                    this.rect_12.Visibility = Visibility.Visible;
                    this.rect_20.Visibility = Visibility.Visible;
                    this.rect_21.Visibility = Visibility.Visible;
                    this.rect_22.Visibility = Visibility.Visible;
                }
            }
        }
        private GeometryCollection Geometries;
        //  private GeometryGroup shapeGroup;
        private MediaBase mediaSource;
        private PlayerMode _player = PlayerMode.None;
        public PlayerMode ModePlayer
        {
            get
            {
                return this._player;
            }
            set
            {
                this._player = value;
                if (value == PlayerMode.None)
                {
                    if (this.videoPlayer.IsPlaying || this.videoPlayer.IsPaused || this.videoPlayer.IsPausable)
                    {
                        this.videoPlayer.Stop();
                    }
                    Canvas.SetZIndex(this.OutLine, 100);
                    Canvas.SetZIndex(this.video, 99);
                    mediaSource = null;
                }
                else
                {
                    Canvas.SetZIndex(this.video, 100);
                    Canvas.SetZIndex(this.OutLine, 99);
                }
                if (value == PlayerMode.Play)
                {
                    if (this.videoPlayer.Media == null)
                    {
                        this.videoPlayer.Media = this.mediaSource;
                    }

                    this.videoPlayer.PositionChanged += vlcControl1_PositionChanged;
                    this.videoPlayer.Play();

                }
                else if (value == PlayerMode.Pause)
                {
                    if (this.videoPlayer.Media != null)
                    {
                        this.videoPlayer.Pause();
                    }
                }
                else if (value == PlayerMode.Stop)
                {
                    if (this.videoPlayer.Media != null)
                    {
                        this.videoPlayer.Stop();
                    }
                }

            }
        }
        private void vlcControl1_PositionChanged(VlcControl sender, Vlc.DotNet.Core.VlcEventArgs<float> e)
        {
            this.videoPlayer.AudioProperties.IsMute = this.isMute;
        }
        private string _media = "";
        public String MediaSource
        {
            get
            {
                return this._media;
            }
            set
            {
                this._media = value;
                mediaSource = new PathMedia(value);
                mediaSource.ParsedChanged += mediaSource_ParsedChanged;
            }
        }
        void mediaSource_ParsedChanged(MediaBase sender, Vlc.DotNet.Core.VlcEventArgs<int> e)
        {

        }
        private bool _isMute = false;

        public bool isMute
        {
            get { return this._isMute; }
            set
            {
                this._isMute = value;
                this.videoPlayer.AudioProperties.IsMute = value;
            }
        }
        private Point delta;
        private bool _isSelected = false;
        public event EventHandler<bool> selectEvent;
        public bool isSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                if (value)
                {
                    this.OutLine.BorderBrush = Brushes.LightSeaGreen;
                }
                else
                {
                    this.OutLine.BorderBrush = Brushes.OrangeRed;
                }
                this.tmpBrush = this.OutLine.BorderBrush;
            }
        }
        private UIElement Parents
        {
            get
            {
                return VisualTreeHelper.GetParent(this) as UIElement;
            }
        }
        public Double RealWidth
        {
            get
            {
                if (this.Width == Double.NaN)
                {
                    return 0;
                }
                if (this.Width < 16)
                    return 0;
                return this.Width - 16;
            }
            set
            {
                if (value < 4)
                    return;
                this.Width = value + 16;
                OutLine.Width = value;
                this.video.Width = value;
                this.layoutDraw.Width = value - 4;
                rect_01.setPosition(value / 2 + 4, 0);
                Canvas.SetLeft(rect_21, value / 2 + 4);
                Canvas.SetBottom(rect_21, 0);

            }
        }
        private void ScaleContent()
        {
            if (this.Children == null)
                return;

            this.layoutDraw.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform s = new ScaleTransform(this.Width / this.fixWidth, this.Height / this.fixHeight);
            this.layoutDraw.LayoutTransform = s;

        }
        public Double RealHeight
        {
            get
            {
                if (this.Height == Double.NaN)
                    return 0;
                if (this.Height < 16)
                    return 0;
                return this.Height - 16;
            }
            set
            {
                if (value < 4)
                    return;
                this.Height = value + 16;
                this.video.Height = value;
                OutLine.Height = value;
                this.layoutDraw.Height = value - 4;
                // ScaleContent();
                rect_10.setPosition(0, value / 2 + 4);
                Canvas.SetRight(rect_12, 0);
                Canvas.SetTop(rect_12, value / 2 + 4);

            }
        }
        public alta_Screen()
        {
            InitializeComponent();
            this.Width = 0;
            this.Height = 0;
        }
        #region resize
        private void resize_top_left(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(-p.Y + this.getPosition().Y + this.RealHeight);
                this.RealWidth = Math.Abs(-p.X + this.getPosition().X + this.RealWidth);
                ScaleContent();
                this.setPosition(p);

            }
        }

        private void CaptrueMouse(object sender, MouseButtonEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            tmp.CaptureMouse();
            tmp.Cursor = Cursors.Hand;
        }

        private void EndResize(object sender, MouseButtonEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            tmp.ReleaseMouseCapture();
        }

        private void resize_Bottom_Right(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(p.Y - this.getPosition().Y);
                this.RealWidth = Math.Abs(p.X - this.getPosition().X);
                ScaleContent();
            }
        }

        private void resize_Bottom_Left(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(p.Y - this.getPosition().Y);
                this.RealWidth = Math.Abs(this.getPosition().X + this.RealWidth - p.X);
                ScaleContent();
                Canvas.SetLeft(this, p.X);
            }
        }

        private void resize_Center_Left(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealWidth = Math.Abs(this.getPosition().X + this.RealWidth - p.X);
                ScaleContent();
                Canvas.SetLeft(this, p.X);
            }
        }

        private void resize_Center_Right(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealWidth = Math.Abs(p.X - this.getPosition().X);
                ScaleContent();
            }
        }

        private void resize_Center_Top(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(-p.Y + this.getPosition().Y + this.RealHeight);
                this.ScaleContent();
                Canvas.SetTop(this, p.Y);
            }
        }

        private void resize_Center_Bottom(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(p.Y - this.getPosition().Y);
                ScaleContent();
            }
        }

        private void resize_top_right(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(-p.Y + this.getPosition().Y + this.RealHeight);
                this.RealWidth = Math.Abs(-this.getPosition().X + p.X);
                ScaleContent();
                Canvas.SetTop(this, p.Y);
            }
        }
        #endregion
        private void Selected(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.isSelected = true;
            }
            else
            {
                this.isSelected = true;
                if (this.selectEvent != null)
                {
                    this.selectEvent(this, this.isSelected);
                }
            }
            this.layoutDraw.ReleaseMouseCapture();
        }

        private void BeginCaptrue(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this.Parents);
            Point pos = this.getPosition();
            this.delta = new Point() { X = p.X - pos.X, Y = p.Y - pos.Y };
            this.layoutDraw.CaptureMouse();
            this.layoutDraw.Cursor = Cursors.SizeAll;
        }

        private void MoveItem(object sender, MouseEventArgs e)
        {
            if (this.layoutDraw.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.setPosition(p.X - this.delta.X, p.Y - this.delta.Y);
            }
        }
        private void AddShape(Shape shape, Geometry geometry = null)
        {
            if (this.Children == null)
            {
                this.Children = new List<Shape>();
            }

            shape.Fill = Brushes.Orange;
            this.Children.Add(shape);
            if (geometry != null)
            {
                if (this.Geometries == null)
                    this.Geometries = new GeometryCollection();
                this.Geometries.Add(geometry);
            }
            this.shapeGroup.Children = this.Geometries;
        }
        public void AddProperty(ShapeProperty @ShapeProperty)
        {
            if (this.Properties == null)
            {
                this.Properties = new List<ShapeProperty>();
            }
            ShapeProperty.setFixCode();
            ShapeProperty.Top = ShapeProperty.FixTop - this.minTop;
            ShapeProperty.Left = ShapeProperty.FixLeft - this.minLeft;
            this.Properties.Add(ShapeProperty);
            switch (ShapeProperty.Type)
            {
                case ShapeChilden.Ellipse:
                    Ellipse shape = new Ellipse();
                    shape.Width = ShapeProperty.Width;
                    shape.Height = ShapeProperty.Height;
                    shape.setPosition(ShapeProperty.Left, ShapeProperty.Top);
                    EllipseGeometry @EllipseGeometry = new EllipseGeometry();
                    EllipseGeometry.Center = new Point() { X = (ShapeProperty.Width) / 2, Y = (ShapeProperty.Height) / 2 };
                    EllipseGeometry.RadiusX = (ShapeProperty.Width) / 2;
                    EllipseGeometry.RadiusY = (ShapeProperty.Height) / 2;
                    this.AddShape(shape, EllipseGeometry);
                    break;
                case ShapeChilden.Arc:
                    ArcProperty @ArcProperty = ShapeProperty as ArcProperty;
                    Arc tmpshape = new Arc();
                    PathGeometry @PathGeometry = getPathGeometry(ArcProperty);
                    tmpshape.ArcThickness = @ArcProperty.ArcThichness;
                    tmpshape.ArcThicknessUnit = ArcProperty.UnitType;
                    tmpshape.EndAngle = ArcProperty.EndAngle;
                    tmpshape.StartAngle = ArcProperty.StartAngle;
                    tmpshape.setPosition(ArcProperty.Left, ArcProperty.Top);
                    tmpshape.Width = ShapeProperty.Width;
                    tmpshape.Height = ShapeProperty.Height;
                    this.AddShape(tmpshape, PathGeometry);
                    break;
                case ShapeChilden.BlockArrow:
                    BlockArrowProperty @BlockArrowProperty = ShapeProperty as BlockArrowProperty;
                    BlockArrow tmpshape2 = new BlockArrow();
                    Geometry BlockArrow = getPathGeometry(BlockArrowProperty);
                    tmpshape2.Orientation = BlockArrowProperty.Orientation;
                    tmpshape2.setPosition(BlockArrowProperty.Left, BlockArrowProperty.Top);
                    tmpshape2.Width = ShapeProperty.Width;
                    tmpshape2.Height = ShapeProperty.Height;
                    this.AddShape(tmpshape2,BlockArrow);
                    break;
                case ShapeChilden.Rectangle:
                    RectangleProperty @RectangleProperty = ShapeProperty as RectangleProperty;
                    Rectangle @Rectangle = new Rectangle();
                    Rectangle.RadiusX = RectangleProperty.RadiusX;
                    Rectangle.RadiusY = RectangleProperty.RadiusY;
                    Rectangle.setPosition(@RectangleProperty.Left, @RectangleProperty.Top);
                    Rectangle.Width = ShapeProperty.Width;
                    Rectangle.Height = ShapeProperty.Height;
                    RectangleGeometry @RectangleGeometry = new RectangleGeometry();
                    RectangleGeometry.RadiusX = RectangleProperty.RadiusX;
                    RectangleGeometry.RadiusY = RectangleProperty.RadiusY;
                    this.AddShape(Rectangle, RectangleGeometry);
                    break;
                case ShapeChilden.RegularPolygon:
                    RegularPolygonProperty @RegularPolygonProperty = ShapeProperty as RegularPolygonProperty;
                   
                    PathGeometry PolygonGeometry = getPathGeometry(RegularPolygonProperty);
                    RegularPolygon tmpShape3 = new RegularPolygon();
                    tmpShape3.PointCount = RegularPolygonProperty.PointCount;
                    tmpShape3.InnerRadius = RegularPolygonProperty.InnerRadius;
                    tmpShape3.setPosition(@RegularPolygonProperty.Left, @RegularPolygonProperty.Top);
                    tmpShape3.Width = ShapeProperty.Width;
                    tmpShape3.Height = ShapeProperty.Height;
                    this.AddShape(tmpShape3, PolygonGeometry);
                    break;
            }
            int count = this.Properties.Count;
            if (count == 1)
            {
                this.maxLeft = this.Properties[0].FixLeft + this.Properties[0].Width;
                this.maxTop =  this.Properties[0].FixTop + this.Properties[0].Height;
                this.minLeft = this.Properties[0].FixLeft;
                this.minTop =  this.Properties[0].FixTop;
            }
            else
            {
                if (this.minLeft > ShapeProperty.FixLeft)
                {
                    this.minLeft = ShapeProperty.FixLeft;
                }
                if (this.minTop > ShapeProperty.FixTop)
                {
                    this.minTop = ShapeProperty.FixTop;
                }
                if (this.maxLeft < ShapeProperty.FixLeft + ShapeProperty.Width)
                {
                    this.maxLeft = ShapeProperty.FixLeft + ShapeProperty.Width;
                }
                if (this.maxTop < ShapeProperty.FixTop + ShapeProperty.Height)
                {
                    this.maxTop = ShapeProperty.FixTop + ShapeProperty.Height;
                }
            }
            this.UpdateDraw();

        }

        private PathGeometry getPathGeometry(RegularPolygonProperty RegularPolygonProperty)
        {
            PathGeometry PathGeometry = new PathGeometry();
            PathFigure Fig = new PathFigure();
            double h = RegularPolygonProperty.Height / 2;
            double w = RegularPolygonProperty.Width / 2;
            Fig.IsClosed = true;
            Fig.StartPoint = new Point(w,0);
            if (RegularPolygonProperty.PointCount <= 3)
            {
                LineSegment line1 = new LineSegment();
                line1.Point = new Point(RegularPolygonProperty.Width, RegularPolygonProperty.Height);
                Fig.Segments.Add(line1);
                LineSegment line2 = new LineSegment();
                line2.Point = new Point(0,RegularPolygonProperty.Height);
                Fig.Segments.Add(line2);
                PathGeometry.Figures.Add(Fig);
                return PathGeometry;

            }
            else if (RegularPolygonProperty.PointCount == 4)
            {
                LineSegment line1 = new LineSegment();
                line1.Point = new Point(RegularPolygonProperty.Width, h);
                Fig.Segments.Add(line1);
                LineSegment line2 = new LineSegment();
                line2.Point = new Point(w, h);
                Fig.Segments.Add(line2);
                LineSegment line3 = new LineSegment();
                line3.Point = new Point(0, h);
                Fig.Segments.Add(line3);
                PathGeometry.Figures.Add(Fig);
                return PathGeometry;
            }
            else if (RegularPolygonProperty.PointCount == 5)
            {
                LineSegment line1 = new LineSegment();
                line1.Point = new Point(RegularPolygonProperty.Width, RegularPolygonProperty.Height*0.4);
                Fig.Segments.Add(line1);
                LineSegment line2 = new LineSegment();
                line2.Point = new Point(w+w/2, RegularPolygonProperty.Height);
                Fig.Segments.Add(line2);
                LineSegment line3 = new LineSegment();
                line3.Point = new Point(w/2, RegularPolygonProperty.Height);
                Fig.Segments.Add(line3);
                PathGeometry.Figures.Add(Fig);
                LineSegment line4 = new LineSegment();
                line4.Point = new Point(0, RegularPolygonProperty.Height * 0.4);
                Fig.Segments.Add(line4);
                PathGeometry.Figures.Add(Fig);
                return PathGeometry;
            }
            else if (RegularPolygonProperty.PointCount == 6)
            {
                LineSegment line1 = new LineSegment();
                line1.Point = new Point(RegularPolygonProperty.Width, h/2);
                Fig.Segments.Add(line1);
                LineSegment line2 = new LineSegment();
                line2.Point = new Point(RegularPolygonProperty.Width, h/2+h);
                Fig.Segments.Add(line2);
                LineSegment line3 = new LineSegment();
                line3.Point = new Point(w, RegularPolygonProperty.Height);
                Fig.Segments.Add(line3);
                PathGeometry.Figures.Add(Fig);
                LineSegment line4 = new LineSegment();
                line4.Point = new Point(0 , h/2+h);
                Fig.Segments.Add(line4);
                LineSegment line5 = new LineSegment();
                line5.Point = new Point(0, h/2);
                Fig.Segments.Add(line5);
                PathGeometry.Figures.Add(Fig);
                return PathGeometry;
            }
            return null;
        }

        private Geometry getPathGeometry(BlockArrowProperty BlockArrowProperty)
        {
            if (BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Left)
            {
                return Geometry.Parse("M0.5,50 L50,0.5 L50,25.25 L99.5,25.25 L99.5,74.75 L50,74.75 L50,99.5 z");
            }
            else if (BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Right)
            {
                return Geometry.Parse("M99.5,50 L50,0.5 L50,25.25 L0.5,25.25 L0.5,74.75 L50,74.75 L50,99.5 z");
            }
            else if (BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Down)
            {
                return Geometry.Parse("M50,99.5 L0.5,50 L25.25,50 L25.25,0.5 L74.75,0.5 L74.75,50 L99.5,50 z");
            }
            else if (BlockArrowProperty.Orientation == Microsoft.Expression.Media.ArrowOrientation.Up)
            {
                return Geometry.Parse("M50,0.5 L0.5,50 L25.25,50 L25.25,99.5 L74.75,99.5 L74.75,50 L99.5,50 z");
            }
            return null;
        }
        


        public void UpdateDraw()
        {
            int count = this.Properties.Count;
            for (int i = 0; i < count; i++)
            {
                this.Children[i].setPosition(this.Properties[i].Left, this.Properties[i].Top);
            }
        }

        private void Usercontrol_Load(object sender, RoutedEventArgs e)
        {
            this.fixWidth = this.Width;
            this.fixHeight = this.Height;
            VisualBrush Mask = new VisualBrush();
            Binding binding = new Binding();
            binding.Source = this.layoutDraw;
            BindingOperations.SetBinding(Mask, VisualBrush.VisualProperty, binding);
            this.video.OpacityMask = Mask;

        }
        public void UpdateMask()
        {
            //VisualBrush Mask = new VisualBrush();
            //Binding binding = new Binding();
            //binding.Source = this.layoutDraw;
            //BindingOperations.SetBinding(Mask, VisualBrush.VisualProperty, binding);
        }
        private PathGeometry getPathGeometry(ArcProperty ShapeProperty)
        {
            PathGeometry PathGeometry = new PathGeometry();
            double R = ShapeProperty.Width / 2;
            Point I = new Point() { X = ShapeProperty.Width / 2, Y = ShapeProperty.Height / 2 };
            double AngelStart = ShapeProperty.StartAngle;
           // List<Point> listPoint = this.getListPoint(ShapeProperty.StartAngle, ShapeProperty.EndAngle, R);
            PathFigure Fig = new PathFigure();
            Fig.IsClosed = true;
            Fig.StartPoint = new Point(99.5, 50);
            
          //  int index = 0;
            BezierSegment BezierSegment1 = new BezierSegment();
            BezierSegment1.IsSmoothJoin = true;
            BezierSegment1.Point1 = new Point(99.5, 70.020881652832);
            BezierSegment1.Point2 = new Point(87.4397125244141, 88.0703735351563);
            BezierSegment1.Point3 = new Point(68.9428329467773, 95.7320327758789);
            Fig.Segments.Add(BezierSegment1);

            BezierSegment BezierSegment2 = new BezierSegment();
            BezierSegment2.IsSmoothJoin = true;
            BezierSegment2.Point1 = new Point(50.4459457397461, 103.393692016602);
            BezierSegment2.Point2 = new Point(29.1551189422607, 99.1586837768555);
            BezierSegment2.Point3 = new Point(14.9982175827026, 85.0017852783203);
            Fig.Segments.Add(BezierSegment2);

            BezierSegment BezierSegment3 = new BezierSegment();
            BezierSegment3.IsSmoothJoin = true;
            BezierSegment3.Point1 = new Point(0.841317534446716, 70.8448867797852);
            BezierSegment3.Point2 = new Point(-3.3936882019043, 49.5540542602539);
            BezierSegment3.Point3 = new Point(4.26796770095825, 31.0571727752686);
            Fig.Segments.Add(BezierSegment3);

            BezierSegment BezierSegment4 = new BezierSegment();
            BezierSegment4.Point1 = new Point(11.9296264648438, 12.5602922439575);
            BezierSegment4.Point2 = new Point(29.979118347168, 0.5);
            BezierSegment4.Point3 = new Point(50, 0.5);
            Fig.Segments.Add(BezierSegment4);

            LineSegment line = new LineSegment();
            line.Point = I;
            /*
            if (listPoint != null && listPoint.Count > 0)
            {
                int count = listPoint.Count;
                while (index < (count - 1) / 3)
                {
                    // int i = index;
                    if (index < 1)
                    {
                        BezierSegment1.Point1 = listPoint[index * 3 + 1];
                        BezierSegment1.Point2 = listPoint[index * 3 + 2];
                        BezierSegment1.Point3 = listPoint[index * 3 + 3];
                        BezierSegment1.IsSmoothJoin = true;
                        Fig.Segments.Add(BezierSegment1);
                    }
                    else if (index < 2)
                    {
                        BezierSegment2.Point1 = listPoint[index * 3 + 1];
                        BezierSegment2.Point2 = listPoint[index * 3 + 2];
                        BezierSegment2.Point3 = listPoint[index * 3 + 3];
                        BezierSegment2.IsSmoothJoin = true;
                        Fig.Segments.Add(BezierSegment2);
                    }
                    else if (index < 3)
                    {
                        
                        BezierSegment3.Point1 = listPoint[index * 3 + 1];
                        BezierSegment3.Point2 = listPoint[index * 3 + 2];
                        BezierSegment3.Point3 = listPoint[index * 3 + 3];
                        BezierSegment3.IsSmoothJoin = true;
                        Fig.Segments.Add(BezierSegment3);
                    }
                    else if (index < 4)
                    {
                        BezierSegment4.Point1 = listPoint[index * 3 + 1];
                        BezierSegment4.Point2 = listPoint[index * 3 + 2];
                        BezierSegment4.Point3 = listPoint[index * 3 + 3];
                        if (ShapeProperty.EndAngle - ShapeProperty.StartAngle >= 360)
                        {
                            BezierSegment4.IsSmoothJoin = true;
                        }
                        Fig.Segments.Add(BezierSegment4);
                    }
                    index++;
                }

            }*/

            if (ShapeProperty.EndAngle-ShapeProperty.StartAngle < 360)
            {
                Fig.Segments.Add(line);
            }
            PathGeometry.Figures.Add(Fig);
            return PathGeometry;
        }

        private List<Point> getListPoint(double angle, double endangel, double R)
        {
          //  double reEnd =endangel.ToRadians();
            double space = endangel - angle;
            int tmp = 0;
            if (space < 90)
            {
                tmp = 3;
            }
            else if (space < 180)
            {
                tmp = 6;
            }
            else if (space < 270)
            {
                tmp = 9;
            }
            else
            {
                tmp = 12;
            }
            double delta = space / (tmp);
            List<Point> listPoint = new List<Point>();
            for (double b = angle;
                b <= endangel; b += delta)
            {
                double a = b.ToRadians();
                Point p = new Point();
                if (a <= Math.PI / 2)
                {
                    p.X =Math.Sin(a) * R + R;
                    p.Y = R - Math.Cos(a) * R;
                }
                else if (a <= Math.PI)
                {
                    p.X = Math.Sin((Math.PI - a)) * R + R;
                    p.Y = Math.Cos((Math.PI - a)) * R + R;

                }
                else if (a <= 3 * Math.PI / 2)
                {
                    p.X = R - Math.Cos((3 * Math.PI / 2 - a)) * R;
                    p.Y = R + Math.Sin((3 * Math.PI / 2 - a)) * R;

                }
                else if (a <= 2 * Math.PI)
                {
                    p.X =R - Math.Sin((2 * Math.PI - a)) * R;
                    p.Y = R - Math.Cos((2 * Math.PI - a)) * R;
                }
                listPoint.Add(p);
            }
            return listPoint;
        }

    }

}
