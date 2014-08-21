using Alta_LED.Data.Init;
using Alta_LED.User_Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Alta_LED.Data.Store
{
    [Serializable]
    public class Template : abs_class_xml
    {
        public List<ShapeTemplate> Children;
        public void Clear()
        {
            this.Children.Clear();
        }
        public int Count
        {
            get
            {
                return this.Children.Count;
            }
        }
        public void remove(ShapeTemplate iTemp)
        {
            if (File.Exists(iTemp.pathImage))
            {
                File.Delete(iTemp.pathImage);
            }
            this.Children.Remove(iTemp);
        }
        public  List<Border_Content> Parse()
        {
            int count = this.Count;
            List<Border_Content> list = new List<Border_Content>();
            for (int i = 0; i < count; i++)
            {
                list.Add(this.Children[i].Parse());
                
            }
            return list;
        }
        public void Add(Border_Content shape, System.Windows.Controls.Canvas Canvas )
        {
            ShapeTemplate tmp = new ShapeTemplate();
            tmp.pathImage = Canvas.saveCanvasToFile();
            tmp.Parse(shape);
            this.Children.Add(tmp);
        }
        public void Add(ShapeTemplate temp, int index)
        {
            int count = this.Count;
            if (index < count)
            {
                this.Children[index] = temp;
            }
            else
            {
                for (int i = this.Count; i <= index; i++)
                {
                    if (i == index)
                    {
                        this.Children.Add(new ShapeTemplate());
                        this.Children[i] = temp;
                        break;
                    }
                    else
                    {
                        this.Add(null);
                    }
                }
            }
        }
        public void Add(ShapeTemplate tmp)
        {
            this.Children.Add(tmp);
        }
        public Template()
        {
            Children = new List<ShapeTemplate>();
            this.fileName = "Data_Save.data";
        }
        ~Template()
        {
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                if (File.Exists(this.Children[i].pathImage))
                {
                    File.Delete(this.Children[i].pathImage);
                }
            }
        }
        public override abs_class_xml loadXml()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Template));
                System.IO.FileStream stream = new System.IO.FileStream(this.fileName + ".xml", System.IO.FileMode.Open);
                var tmp = (Template)ser.Deserialize(stream);
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
