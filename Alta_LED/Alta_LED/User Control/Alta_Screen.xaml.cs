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
        private void AddShape(Shape shape)
        {
            if (this.Children == null)
            {
                this.Children = new List<Shape>();
            }

            shape.Fill = Brushes.Orange;
            this.Children.Add(shape);
            this.layoutDraw.Children.Add(this.Children[this.Children.Count - 1]);
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
                    this.AddShape(shape);
                    break;
                case ShapeChilden.Arc:
                    ArcProperty @ArcProperty = ShapeProperty as ArcProperty;
                    Arc tmpshape = new Arc();

                    tmpshape.ArcThickness = @ArcProperty.ArcThichness;
                    tmpshape.ArcThicknessUnit = ArcProperty.UnitType;
                    tmpshape.EndAngle = ArcProperty.EndAngle;
                    tmpshape.StartAngle = ArcProperty.StartAngle;
                    tmpshape.setPosition(ArcProperty.Left, ArcProperty.Top);
                    tmpshape.Width = ShapeProperty.Width;
                    tmpshape.Height = ShapeProperty.Height;
                    this.AddShape(tmpshape);
                    break;
                case ShapeChilden.BlockArrow:
                    BlockArrowProperty @BlockArrowProperty = ShapeProperty as BlockArrowProperty;
                    BlockArrow tmpshape2 = new BlockArrow();
                    tmpshape2.Orientation = BlockArrowProperty.Orientation;
                    tmpshape2.setPosition(BlockArrowProperty.Left, BlockArrowProperty.Top);
                    tmpshape2.Width = ShapeProperty.Width;
                    tmpshape2.Height = ShapeProperty.Height;
                    this.AddShape(tmpshape2);
                    break;
                case ShapeChilden.Rectangle:
                    RectangleProperty @RectangleProperty = ShapeProperty as RectangleProperty;
                    Rectangle @Rectangle = new Rectangle();
                    Rectangle.RadiusX = RectangleProperty.RadiusX;
                    Rectangle.RadiusY = RectangleProperty.RadiusY;
                    Rectangle.setPosition(@RectangleProperty.Left, @RectangleProperty.Top);
                    Rectangle.Width = ShapeProperty.Width;
                    Rectangle.Height = ShapeProperty.Height;
                    this.AddShape(Rectangle);
                    break;
                case ShapeChilden.RegularPolygon:
                    RegularPolygonProperty @RegularPolygonProperty = ShapeProperty as RegularPolygonProperty;
                    RegularPolygon tmpShape3 = new RegularPolygon();
                    tmpShape3.PointCount = RegularPolygonProperty.PointCount;
                    tmpShape3.InnerRadius = RegularPolygonProperty.InnerRadius;
                    tmpShape3.setPosition(@RegularPolygonProperty.Left, @RegularPolygonProperty.Top);
                    tmpShape3.Width = ShapeProperty.Width;
                    tmpShape3.Height = ShapeProperty.Height;
                    this.AddShape(tmpShape3);
                    break;
            }
            int count = this.Properties.Count;
            if (count == 1)
            {
                this.maxLeft = this.Properties[0].FixLeft + this.Properties[0].Width;
                this.maxTop = this.Properties[0].FixTop + this.Properties[0].Height;
                this.minLeft = this.Properties[0].FixLeft;
                this.minTop = this.Properties[0].FixTop;


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


    }

}
