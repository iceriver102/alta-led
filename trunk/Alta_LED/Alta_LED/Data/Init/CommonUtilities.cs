using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alta_LED.Data.Init
{
    public static class CommonUtilities
    {
        public static setting setting ;
        public static Alta_Size sizeDesign = new Alta_Size();

        public static double width { get; set; }

        public static double height { get; set; }

        internal static System.Windows.Size getScaleSize()
        {
            return new System.Windows.Size(width / sizeDesign.Width, height / sizeDesign.Height);
        }
        public static TraceSource myLog = new TraceSource("AltaLED");
        public static void WriteLog(String str,int id=0, TraceEventType type=TraceEventType.Error)
        {
            myLog.TraceEvent(type, id, str);
        }
        public static void WriteLog(Object obj)
        {

        }

    }
}
