using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Alta_LED.Data.Themes
{
    partial class SimpleTemplate : ResourceDictionary
    {
       public SimpleTemplate()
       {
          InitializeComponent();
       }

       public void CloseClick(object sender, RoutedEventArgs e)
       {
           Window mWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
           string tabName = (sender as Button).CommandParameter.ToString();
           Type mType = mWindow.GetType();
           object[] mParams = new object[] { tabName };
           MethodInfo mMethodInfo = mType.GetMethod("CloseTab");
           if (mMethodInfo != null)
           {
               mMethodInfo.Invoke(mWindow, mParams);
           }
       }
    }
}
