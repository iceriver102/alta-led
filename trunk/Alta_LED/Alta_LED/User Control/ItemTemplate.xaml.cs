using Alta_LED.Data.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alta_LED
{
	/// <summary>
	/// Interaction logic for ItemTemplate.xaml
	/// </summary>
	public partial class ItemTemplate : UserControl
	{
        public bool _hasTemplate;
        public bool hasTemplate
        {
            get
            {
                return this._hasTemplate;
            }
            set
            {
                this._hasTemplate = value;
                if (value)
                {
                    this.lb_hasTemplate.Visibility = Visibility.Visible;
                }
                else
                {
                    this.lb_hasTemplate.Visibility = Visibility.Hidden;
                }
            }
        }
        public UIElement IParent
        {
            get
            {
                return VisualTreeHelper.GetParent(this) as UIElement;
            }
        }
        private Template data;
        public Template Data
        {
            get
            {
                return this.data;
            }
            set
            {
                if (value!=null && value.Count > 0)
                {
                    this.data = value;
                    this.hasTemplate = true;
                }
                else
                {
                    this.hasTemplate = false;
                    this.data = null;
                }
            }
        }
        public event EventHandler<Template> LoadTemplate;
        public event EventHandler DeleteClick;
        public event EventHandler Click;
        public bool _selected = false;
        public event EventHandler<bool> Selected;
        private Brush BorderTmp;
        public bool isSelected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                if (value)
                {
                    if(this.Selected!=null)
                     this.Selected(this, value);
                    this.Out_Line.BorderBrush = Brushes.LightBlue;
                }
                else
                {
                    this.Out_Line.BorderBrush = Brushes.Black;
                }
                BorderTmp = this.Out_Line.BorderBrush;
            }
        }
        private static DropShadowEffect shadow= new DropShadowEffect(){ BlurRadius=14, Color= new Color(){A=255,G=71,R=71,B=71}, ShadowDepth=3, Direction=289, Opacity=0.5};
		public ItemTemplate()
		{
			this.InitializeComponent();
            this.Cursor = Cursors.Hand;
            this.hasTemplate = false;
            
		}

        private void setting_click(object sender, RoutedEventArgs e)
        {

        }
        private void window_mouse_enter(object sender, MouseEventArgs e)
        {
            BorderTmp = this.Out_Line.BorderBrush;
            this.Out_Line.BorderBrush = new SolidColorBrush(new Color() { A = 255, R = 255, G = 104, B = 0 });
            this.Out_Line.BorderThickness = new Thickness(1.5);
            this.Effect = shadow;
        }

        private void window_mouse_leave(object sender, MouseEventArgs e)
        {
            this.Out_Line.BorderBrush = BorderTmp;
            this.Out_Line.BorderThickness = new Thickness(1);
            this.Effect = null;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            this.Data = null;

        }
        private void Click_Event(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
            {
                Click(this, new EventArgs());
            }
            if (this.hasTemplate && this.LoadTemplate!=null)
            {
                this.LoadTemplate(this, this.Data);
            }
            this.isSelected = true;
        }
	}
}