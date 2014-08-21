using Alta_LED.Data.Init;
using Alta_LED.User_Control;
using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Alta_LED.Data.Store
{
    [Serializable]
     public class ShapeTemplate
    {
        public Double Width;
        public Double Height;
        public Double Top;
        public Double Left;
        public Double Right;
        public Double Bottom;

        public String MediaSource;
        public bool isSelected;
        public bool isMute;
        public ShapeProperty Property;
        public String pathImage;
        public ImageSource Image
        {
            get
            {
                return ImageHelpers.getbitmapImage(this.pathImage);
            }
           
        }
        ~ShapeTemplate()
        {
            if (File.Exists(this.pathImage))
            {
                File.Delete(this.pathImage);
            }
        }
        public void Parse(Border_Content content)
        {
            this.Width = content.RealWidth;
            this.Height = content.RealHeight;
            this.Top = (Double)content.GetValue(Canvas.TopProperty);
            this.Left = (Double)content.GetValue(Canvas.LeftProperty);
            this.isMute = content.isMute;           
            this.MediaSource = content.MediaSource;
            this.isSelected = content.isSelected;
            this.Property = content.Property;
        }
        public Border_Content Parse()
        {
            Border_Content content = new Border_Content();        
            content.Property = this.Property;
            content.isMute = this.isMute;
            content.setPosition(this.Left,this.Top);
            content.MediaSource = this.MediaSource;
            return content;
        }
    }
}
