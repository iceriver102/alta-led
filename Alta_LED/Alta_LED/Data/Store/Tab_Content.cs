using Alta_LED.Data.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alta_LED.Data.Store
{
    [Serializable]
    public class Tab_Content : abs_class_xml
    {
        public List<Template> Children;
        public int Count
        {
            get
            {
                return this.Children.Count;
            }
        }

        public Tab_Content()
        {
            this.Children = new List<Template>();
            this.fileName = "Tab_Content.data";
        }

        public void Add(Template temp)
        {
            this.Children.Add(temp);
        }
        public void Clear()
        {
            this.Children.Clear();
        }
        public void remove(Template temp)
        {
            this.Children.Remove(temp);
        }
        public void removeAt(int i)
        {
            if (this.Count > i)
                this.Children.RemoveAt(i);
        }


        public override abs_class_xml loadXml()
        {
            return null;
        }
    }
}
