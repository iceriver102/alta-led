using Alta_LED.Data.Init;
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

namespace Alta_LED.User_Control
{
    /// <summary>
    /// Interaction logic for Alta_Screen.xaml
    /// </summary>
    public partial class Alta_Screen : UserControl
    {
        private UIElement _parent;
        public event EventHandler RemoveEvent;
        public event EventHandler<bool> SelectChange;
        private Point p;
        private bool _is_select;
        public bool isSelected
        {
            get
            {
                return this._is_select;
            }
            set
            {
                if (value)
                {
                    this.outLine.BorderBrush = Brushes.Blue;
                }
                else
                {
                    this.outLine.BorderBrush = Brushes.Black;
                }
                if (this._is_select != value)
                {
                    this._is_select = value;
                    if (this.SelectChange != null)
                    {
                        this.SelectChange(this, this._is_select);
                    }
                }
            }
        }
        public UIElement Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
                if (this._parent is Canvas)
                {
                    (this._parent as Canvas).Children.Add(this);
                }
                else
                {
                   
                }
            }
        }
        public Alta_Screen()
        {
            InitializeComponent();
            this._is_select = false;
        }

        private void Window_PreviewDown(object sender, MouseButtonEventArgs e)
        {
            grid_fix.CaptureMouse();
            p = e.GetPosition(this.Parent);
        }

        private void ScreenMove(object sender, MouseEventArgs e)
        {
            if (grid_fix.IsMouseCaptured)
            {
                Vector v = new Vector(this.p.X - this.getPosition().X, this.p.Y - this.getPosition().Y);
                this.setPosition(e.GetPosition(this.Parent).X-v.X,e.GetPosition(this.Parent).Y-v.Y);
                this.p = e.GetPosition(this.Parent);
            }
        }

        private void EndMove(object sender, MouseButtonEventArgs e)
        {
            this.grid_fix.ReleaseMouseCapture();
        }

        private void Screen_Remove(object sender, RoutedEventArgs e)
        {
            if (RemoveEvent != null)
            {
                RemoveEvent(this, new EventArgs());
            }
        }
        private void Screen_select(object sender, MouseButtonEventArgs e)
        {
            this.isSelected = true;
        }

        private void Resize(object sender, MouseEventArgs e)
        {
            if (Gridresize.IsMouseCaptured)
            {
                this.Height = Math.Abs( e.GetPosition(this.Parent).Y - this.getPosition().Y);
                this.Width = Math.Abs(e.GetPosition(this.Parent).X - this.getPosition().X);
                if (this.Height < this.MinHeight)
                {
                    this.Height = this.MinHeight;
                }
                if (this.Width < this.MinWidth)
                {
                    this.Width = this.MinWidth;
                }
            }
        }

        private void End_Resize(object sender, MouseButtonEventArgs e)
        {
            Gridresize.ReleaseMouseCapture();
        }

        private void Begin_Resize(object sender, MouseButtonEventArgs e)
        {
            Gridresize.CaptureMouse();
        }
    }
}
