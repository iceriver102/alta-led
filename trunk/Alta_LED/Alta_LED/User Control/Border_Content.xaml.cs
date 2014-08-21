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
    /// Interaction logic for Border_Content.xaml
    /// </summary>
    /// 
    [Serializable]
    public partial class Border_Content : UserControl
    {
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
        private bool _run = false;
        private Brush tmpBrush;
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
        private ShapeProperty _property;
        public ShapeProperty Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
                this.RealHeight = value.Height;
                this.RealWidth = value.Width;
                switch (value.Type)
                {
                    case ShapeChilden.Ellipse:
                        Ellipse shape = new Ellipse();
                        this.Child = shape;
                        break;
                    case ShapeChilden.Arc:
                        ArcProperty @ArcProperty = this.Property as ArcProperty;
                        Arc tmpshape = new Arc();
                        tmpshape.ArcThickness = @ArcProperty.ArcThichness;
                        tmpshape.ArcThicknessUnit = ArcProperty.UnitType;
                        tmpshape.EndAngle = ArcProperty.EndAngle;
                        tmpshape.StartAngle = ArcProperty.StartAngle;
                        this.Child = tmpshape;
                        break;
                    case ShapeChilden.BlockArrow:
                        BlockArrowProperty @BlockArrowProperty = this.Property as BlockArrowProperty;
                        BlockArrow tmpshape2 = new BlockArrow();
                        tmpshape2.Orientation = BlockArrowProperty.Orientation;
                        this.Child = tmpshape2;
                        break;
                    case ShapeChilden.Rectangle:
                        RectangleProperty @RectangleProperty = this.Property as RectangleProperty;
                        Rectangle @Rectangle = new Rectangle();
                        Rectangle.RadiusX = RectangleProperty.RadiusX;
                        Rectangle.RadiusY = RectangleProperty.RadiusY;
                        this.Child = Rectangle;
                        break;
                    case ShapeChilden.RegularPolygon:
                        RegularPolygonProperty @RegularPolygonProperty = this.Property as RegularPolygonProperty;
                        RegularPolygon tmpShape3 = new RegularPolygon();
                        tmpShape3.PointCount = RegularPolygonProperty.PointCount;
                        tmpShape3.InnerRadius = RegularPolygonProperty.InnerRadius;
                        this.Child = tmpShape3;
                        break;
                }
                this.Child.setPosition(value.Left, value.Left);


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
                    if (this.selectEvent != null)
                    {
                        this.selectEvent(this, value);
                    }
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
        [NonSerialized]
        private Shape _child;

        public Shape Child
        {
            get
            {
                return this._child;
            }
            set
            {
                value.Fill = Brushes.Orange;
                value.Margin = new Thickness(0);
                if (this.Property != null)
                {
                    value.setPosition(this.Property.Left, this.Property.Top);
                }
                value.Width = this.RealWidth-4;
                value.Height = this.RealHeight-4;
                this._child = value;
                
                VisualBrush Mask = new VisualBrush();
                Binding binding = new Binding();
                binding.Source = value;
                BindingOperations.SetBinding(Mask, VisualBrush.VisualProperty, binding);
                this.video.OpacityMask = Mask;
                this.layoutDraw.Children.Clear();
                this.layoutDraw.Children.Add(value);
            }
        }

        public Double RealWidth
        {
            get
            {
                return this.Width - 16;
            }
           private set
            {
                this.Width = value + 16;
                OutLine.Width = value;
                this.video.Width = value;
                this.layoutDraw.Width = value - 4;
                if (this._child != null)
                {
                    this._child.Width = value - 4;

                }
                rect_01.setPosition(value / 2 + 4, 0);
                Canvas.SetLeft(rect_21, value / 2 + 4);
                Canvas.SetBottom(rect_21, 0);

            }
        }
        public Double RealHeight
        {
            get
            {
                return this.Height - 16;
            }
            private set
            {
                this.Height = value + 16;
                this.video.Height = value;
                OutLine.Height = value;
                this.layoutDraw.Height = value - 4;
                if (this._child != null)
                {
                    this._child.Height = value - 4;
                }
                rect_10.setPosition(0, value / 2 + 4);
                Canvas.SetRight(rect_12, 0);
                Canvas.SetTop(rect_12, value / 2 + 4);
            }
        }
        public Border_Content()
        {
            InitializeComponent();
            this.zIndex = 100;
            this.isMute = false;
        }

        private void resize_top_left(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(-p.Y + this.getPosition().Y + this.RealHeight);
                this.RealWidth = Math.Abs(-p.X + this.getPosition().X + this.RealWidth);
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
            }
        }

        private void resize_Center_Top(object sender, MouseEventArgs e)
        {
            Rectangle tmp = sender as Rectangle;
            if (tmp.IsMouseCaptured)
            {
                Point p = e.GetPosition(this.Parents);
                this.RealHeight = Math.Abs(-p.Y + this.getPosition().Y + this.RealHeight);
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
                Canvas.SetTop(this, p.Y);
            }
        }

        private void Selected(object sender, MouseButtonEventArgs e)
        {
            this.isSelected = true;
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
    }

    public enum PlayerMode
    {
        Play, Pause, Stop, None
    }
}
