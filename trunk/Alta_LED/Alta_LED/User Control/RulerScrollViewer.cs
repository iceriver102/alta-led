using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alta_LED.User_Control
{
    public class RulerScrollViewer : ScrollViewer
    {
        static RulerScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RulerScrollViewer), new FrameworkPropertyMetadata(typeof(RulerScrollViewer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ScrollChanged -= RulerScrollViewer_ScrollChanged;
            this.ScrollChanged += RulerScrollViewer_ScrollChanged;
        }

        void RulerScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var h = this.Template.FindName("PART_HorizontalRulerScrollViewer", this) as ScrollViewer;

            if (h != null)
                h.ScrollToHorizontalOffset(this.HorizontalOffset);

            var v = this.Template.FindName("PART_VerticalRulerScrollViewer", this) as ScrollViewer;
            if (v != null)
                v.ScrollToVerticalOffset(this.VerticalOffset);
        }
    }

    public class HorizontalRuler : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < this.ActualWidth / 100; i++)
            {
                var ft = new FormattedText(i.ToString(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Tahoma"), 8, Brushes.Black);
                drawingContext.DrawText(ft, new Point(i * 100, 0));
            }
        }
    }

    public class VerticallRuler : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < this.ActualHeight / 100; i++)
            {
                var ft = new FormattedText(i.ToString(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Tahoma"), 8, Brushes.Black);
                drawingContext.DrawText(ft, new Point(0, i * 100));
            }
        }
    }
}
