using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Alta_LED.Data.Init
{
    public enum Themes
    {
        Manual,Auto
    }

    public enum App_Mode
    {
        Debug,Run,Demo
    }
    public enum FileTypeImage
    {
        JPG,PNG
    }

    public enum Mode_Control
    {
        Touch, Kinect, None, Touch_Kinect
    }

    [Serializable]
    public class Alta_Size
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    [Serializable]
    public class setting : abs_class_xml
    {
        public int countKinect = 5;
        public int indexDevice = 0;
        public string dataFolder = "Data";
        public Stretch stretch = Stretch.UniformToFill;
        public App_Mode mode = App_Mode.Debug;
        public Alta_Size size = new Alta_Size() { Width = 1366, Height = 768 };
        public Mode_Control modeControl = Mode_Control.Touch;
        public int camerafps=24;
        public int cameraCountTime = 5;
        public FileTypeImage fileType = FileTypeImage.PNG;
        public string Email = "giang.phan@alta.com.vn";
        public string TitleEmail = "Altamedia";
        public string PassEmail = "25.25.1325";
        public string emailSubject = "Altamedia";
        public string emailBody = "<html><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 13px; line-height: normal; text-align: center;\">	<b><font color=\"#000000\" size=\"6\">Souvenir picture from TechDay - Alta Media</font></b></div><div style=\"font-family: arial, sans-serif; font-size: 13px; line-height: normal; color: rgb(80, 0, 80); text-align: center;\">	&nbsp;</div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 13px; line-height: normal; text-align: center;\">	<i><font color=\"#999999\"><font style=\"font-size: small;\">Here is a picture you took at</font><font style=\"font-size: small;\">&nbsp;</font></font></i><b style=\"color: rgb(80, 0, 80); font-size: large; text-align: left;\"><span style=\"line-height: 20.700000762939453px; color: rgb(227, 108, 10);\"><i>TechDay - Alta</i></span></b><i><span style=\"font-size: small;\"><font color=\"#999999\">&nbsp;in HO CHI MINH City on Tuesday, August 19th 2014</font></span></i></div><div style=\"color: rgb(34, 34, 34); font-family: arial, sans-serif; font-size: 13px; line-height: normal; text-align: center;\">	<i><span style=\"font-size: small;\"><font color=\"#999999\">	<img alt=\"Inline image 1\" height=\"394\" src=\"cid:{1}\" style=\"margin-right: 0px;\" width=\"606\" /></font></span></i></div></html>";
        public string fbMessage = "demo";
        public int stickerNum = 3;
        public int accessoriesNum = 7;
        public Themes theme = Themes.Auto;

        public setting(string fileName)
            : base(fileName)
        {
            
        }

        public setting()
        {
            this.fileName = "setting.data";
        }
        public override abs_class_xml loadXml()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(setting));
                FileStream stream = new FileStream(this.fileName+".xml", FileMode.Open);
                var tmp = (setting)ser.Deserialize(stream);
                stream.Close();
                return tmp;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        
    }
}
