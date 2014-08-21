using Alta_LED.Data.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alta_LED.User_Control
{
    /// <summary>
    /// Interaction logic for Grid_Content.xaml
    /// </summary>
    public partial class Grid_Content : UserControl
    {
        public event EventHandler<Alta_LED.Data.Store.Template> LoadTemplate;
        public Grid_Content()
        {
            InitializeComponent();
        }

        public Tab_Content Parse()
        {
            bool flag = false;
            Tab_Content list = new Tab_Content();           
            int count =this.listBlock.Items.Count;
            for (int i = 0; i < count; i++)
            {
                Template temp = new Template();
                if (this.listBlock.Items[i] is ItemTemplate)
                {
                    ItemTemplate iTemp = this.listBlock.Items[i] as ItemTemplate;
                    if (iTemp.hasTemplate)
                    {
                        flag = true;
                        temp = iTemp.Data;
                    }
                    list.Add(temp);
                }
            }
            if (flag)
                return list;
            else
                return null;
        }

        
        private void Item_Selected(object sender, bool e)
        {
            ItemTemplate item = sender as ItemTemplate;
            if (e)
            {
                int count = this.listBlock.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.listBlock.Items[i] is ItemTemplate)
                    {
                        ItemTemplate tmp = this.listBlock.Items[i] as ItemTemplate;
                        if (tmp != item && tmp.isSelected)
                        {
                            tmp.isSelected = false;
                        }
                    }
                }
            }
        }

        private void listBlock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBlock.SelectedIndex != -1)
                this.listBlock.SelectedIndex = -1;
        }

        private void ItemTemplate_LoadTemplate(object sender, Data.Store.Template e)
        {
            if (this.LoadTemplate != null)
            {
                this.LoadTemplate(sender, e);
            }
        }
    }
}
