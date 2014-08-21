using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alta_LED.Data.Init
{
    public class percentScale : ObservableCollection<string>
    {
        public percentScale()
        {
            Add("100");
            Add("50");
            Add("30");
            Add("10");
           
        }
    }
}
