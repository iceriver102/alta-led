using Alta_LED.Data.Init;
using Alta_LED.User_Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Alta_LED.Data.Store
{
    [Serializable]
    public class Matrix_Template : abs_class_xml
    {

        public List<Tab_Content> Children;
        public int Count
        {
            get
            {
                return this.Children.Count;
            }
        }
        public void Add(Tab_Content list)
        {
            if(list!=null)
                this.Children.Add(list);
        }
        public void Add(Template temp,int index=0)
        {
            this.Children[index].Add(temp);
        }

        public void remove(Tab_Content lTemp)
        {
            this.Children.Remove(lTemp);
            
        }
        public Matrix_Template()
        {
            this.Children = new List<Tab_Content>();
            this.fileName = "Data.data";
        }


        public override abs_class_xml loadXml()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Matrix_Template));
                System.IO.FileStream stream = new System.IO.FileStream(this.fileName + ".xml", System.IO.FileMode.Open);
                var tmp = (Matrix_Template)ser.Deserialize(stream);
                stream.Close();
                return tmp;
            }
            catch (Exception ex)
            {

            }
            return new Matrix_Template();
        }
    }
}
