using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Alta_LED.Data.Init
{
    
    [Serializable]
    public abstract class abs_class_xml
    {
        [NonSerialized]
        protected string fileName;
        public abs_class_xml(string fileName)
        {
            this.fileName = fileName;
        }
        public abs_class_xml()
        {

        }
        public void saveXml()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(this.GetType());
                FileStream stream = new FileStream(this.fileName + ".xml", FileMode.Create);
                ser.Serialize(stream, this);
                stream.Close();
            }
            catch (Exception ex)
            {
                CommonUtilities.WriteLog(ex.ToString(), 2, TraceEventType.Error); 
            }
        }
        public abstract abs_class_xml loadXml();
    }
}
