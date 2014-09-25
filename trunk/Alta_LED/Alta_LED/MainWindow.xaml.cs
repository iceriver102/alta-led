using Alta_LED.Data.Init;
using Alta_LED.Data.Store;
using Alta_LED.User_Control;
using Microsoft.Expression.Shapes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Alta_LED
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Template iTemplate;
        Double Layout_Height
        {
            get
            {
                return this.main_layout.Height;
            }
            set
            {
                this.main_layout.Height = value;
            }
        }
        Double Layout_Width
        {
            get
            {
                return this.main_layout.Width;
            }
            set
            {
                this.main_layout.Width = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            iTemplate = new Template();
            CommonUtilities.sizeDesign.Width = (double)this.Resources["D_Width"];
            CommonUtilities.sizeDesign.Height = (double)this.Resources["D_Height"];
            this.SourceInitialized += new EventHandler(Window1_SourceInitialized);
            this.LoadGUID();
        }

       
        public void LoadGUID()
        {
         
            if (App.dataResource != null)
            {
                int count = App.dataResource.Count;
                if (count > 0)
                {
                    initTab(App.dataResource.Children[0]);
                    for (int i = 1; i < count; i++)
                    {
                        AddTabEvent(null, null);
                        initTab(App.dataResource.Children[i], i);
                    }
                }
            }
        }
        public void initTab(Tab_Content list, int index = 0)
        {
            if (list != null)
            {
                TabItem item = this.BankContainer.Items[index] as TabItem;
                Grid_Content content = item.Content as Grid_Content;
                int BlockCount = content.listBlock.Items.Count;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i < BlockCount)
                    {
                        if (content.listBlock.Items[i] is ItemTemplate)
                        {
                            ItemTemplate iTemplate = content.listBlock.Items[i] as ItemTemplate;
                            iTemplate.Data = list.Children[i];
                           //  content.listBlock.Items[i] = iTemplate;
                        }
                    }
                    else
                    {
                        ItemTemplate iTemplate = new ItemTemplate();
                        iTemplate.Data = list.Children[i];
                        iTemplate.Width = 180;
                        iTemplate.Height = 120;
                        iTemplate.Selected += iTemplate_Selected;
                        content.listBlock.Items.Add(iTemplate);
                    }
                }
            }
        }

        void iTemplate_Selected(object sender, bool e)
        {
            ItemTemplate item = sender as ItemTemplate;
            System.Windows.Controls.ListView listBlock = item.IParent as ListView;
            if (e)
            {
                int count = listBlock.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    if (listBlock.Items[i] is ItemTemplate)
                    {
                        ItemTemplate tmp = listBlock.Items[i] as ItemTemplate;
                        if (tmp != item && tmp.isSelected)
                        {
                            tmp.isSelected = false;
                        }
                    }
                }
            }
        }
        public void ItemClick(Object sender, ShapeProperty e)
        {
            Border_Content content = null;
            content = new Border_Content();           
            content.setPosition(0, 0);
            content.selectEvent += content_selectEvent;
            content.Property = e;
            this.main_layout.Children.Add(content);
        }

        void content_selectEvent(object sender, bool e)
        {
            if (e)
            {
                if (sender is Border_Content)
                {
                    Border_Content tmp = sender as Border_Content;
                    if (!tmp.isMute)
                    {
                        this.btn_Volume.Content = "\uf028";
                    }
                    else
                    {
                        this.btn_Volume.Content = "\uf026";
                    }
                    int count = this.main_layout.Children.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (this.main_layout.Children[i] is Border_Content)
                        {
                            Border_Content contentTmp = this.main_layout.Children[i] as Border_Content;
                            if (tmp != contentTmp)
                            {
                                contentTmp.isSelected = false;
                            }

                        }
                        else if (this.main_layout.Children[i] is alta_Screen)
                        {
                            alta_Screen alta_ScreenTmp = this.main_layout.Children[i] as alta_Screen;
                          
                                alta_ScreenTmp.isSelected = false;
                            
                        }
                    }
                }
                else if (sender is alta_Screen)
                {
                    alta_Screen @alta_Screen =sender as alta_Screen;
                    if (alta_Screen.isMute)
                    {
                        this.btn_Volume.Content = "\uf026";
                    }
                    else
                    {
                        this.btn_Volume.Content = "\uf028";
                    }
                    int count = this.main_layout.Children.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (this.main_layout.Children[i] is alta_Screen)
                        {
                            alta_Screen alta_ScreenTmp = this.main_layout.Children[i] as alta_Screen;
                            if (alta_Screen != alta_ScreenTmp)
                            {
                                alta_ScreenTmp.isSelected = false;
                            }
                        }
                        else if (this.main_layout.Children[i] is Border_Content)
                        {
                            Border_Content @Border_Content = this.main_layout.Children[i] as Border_Content;
                            Border_Content.isSelected = false;
                        }
                    }
                }
            }
        }

        private List<TabItem> _tabItems
        {
            get
            {
                return this.BankContainer.Items.OfType<TabItem>().ToList();
            }
        }

        private void fixResoulution()
        {
            CommonUtilities.width = this.Width;
            CommonUtilities.height = this.Height;
            Size scale = CommonUtilities.getScaleSize();
            ScaleTransform s = new ScaleTransform(scale.Width, scale.Height);
            this.canvas_root.RenderTransform = s;
        }

        private void AddTabEvent(object sender, MouseButtonEventArgs e)
        {
            TabItem item = new TabItem();
            item.Name = "Tabitem" + this.BankContainer.Items.Count;
            item.Style = (Style)Application.Current.Resources["SimpleTabItem"];
            item.Header = String.Format("Bank {0}", this.BankContainer.Items.Count);

            Grid_Content grContent = new Grid_Content();
            grContent.LoadTemplate += Grid_Content_LoadTemplate;
            item.Content = grContent;
            this.BankContainer.Items.Insert(this.BankContainer.Items.Count - 1, item);
            this.BankContainer.SelectedIndex = this.BankContainer.Items.Count - 2;
        }
        public void CloseTab(String tabName)
        {
            int count = this._tabItems.Count;
            var item = BankContainer.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();
            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (this._tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = BankContainer.SelectedItem as TabItem;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = BankContainer.Items.GetItemAt(0) as TabItem;
                    }
                    BankContainer.SelectedItem = selectedTab;
                    BankContainer.Items.Remove(tab);
                }
            }
        }

        private void tabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
        }

        private void window_load(object sender, RoutedEventArgs e)
        {
            this.fixResoulution();
            this.Load_Template();

        }
      

        private void window_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void window_minimize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }
        private void Window_Keyup(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    this.delete_Layout(null, null);
                    break;
                case Key.E:
                    String xaml = XamlWriter.Save(this.main_layout);
                    MessageBox.Show(xaml);                    
                    break;
                case Key.L:
                    repeat_click(null,null);
                    break;
            }
        }

        private void Add_video(object sender, RoutedEventArgs e)
        {
            String file = this.OpenMedia();
            if (file != null)
            {
                int count = this.main_layout.Children.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this.main_layout.Children[i] is Border_Content)
                    {
                        Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                        if (tmp.isSelected)
                        {
                           
                            tmp.MediaSource = file;
                            //tmp.ModePlayer = PlayerMode.Play;
                        }
                    }
                    else if (this.main_layout.Children[i] is alta_Screen)
                    {
                        alta_Screen @alta_Screen = this.main_layout.Children[i] as alta_Screen;
                        if (alta_Screen.isSelected)
                        {
                            alta_Screen.MediaSource = file;
                        }
                    }
                }
            }
        }
        private String OpenMedia()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Open media file for playback",
                Filter = "All files |*.*"
            };
            if (openFileDialog.ShowDialog() != true)
                return null;
            return openFileDialog.FileName;
        }

        private void Play_video(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.MediaSource != null)
                    {
                        tmp.ModePlayer = PlayerMode.Play;
                    }
                }
                else if (this.main_layout.Children[i] is alta_Screen)
                {
                    alta_Screen @alta_Screen = this.main_layout.Children[i] as alta_Screen;
                    if (alta_Screen.MediaSource != null)
                    {
                        alta_Screen.ModePlayer = PlayerMode.Play;
                    }
                }
            }
        }

        private void Pause_Video(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.MediaSource != null)
                    {
                        tmp.ModePlayer = PlayerMode.Pause;
                    }
                }
                else if (this.main_layout.Children[i] is alta_Screen)
                {
                    alta_Screen @alta_Screen = this.main_layout.Children[i] as alta_Screen;
                    if (alta_Screen.MediaSource != null)
                    {
                        alta_Screen.ModePlayer = PlayerMode.Pause;
                    }
                }
            }
        }

        private void Stop_Video(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.MediaSource != null)
                    {
                        tmp.ModePlayer = PlayerMode.Stop;
                    }
                }
                else if (this.main_layout.Children[i] is alta_Screen)
                {
                    alta_Screen @alta_Screen = this.main_layout.Children[i] as alta_Screen;
                    if (alta_Screen.MediaSource != null)
                    {
                        alta_Screen.ModePlayer = PlayerMode.Stop;
                    }
                }
            }
        }
        private void Save_Template(object sender, RoutedEventArgs e)
        {
            
            int count = this.main_layout.Children.Count;
            int xIndex = this.BankContainer.SelectedIndex;
            TabItem Tab = this.BankContainer.SelectedItem as TabItem;
            Grid_Content content = Tab.Content as Grid_Content;
            ItemTemplate iTemp = null;
            int yIndex = -1;
            int CountListBlock = content.listBlock.Items.Count;
            for (int i = 0; i < CountListBlock; i++)
            {
                if (content.listBlock.Items[i] is ItemTemplate)
                {
                    ItemTemplate tmp = content.listBlock.Items[i] as ItemTemplate;
                    if (tmp.isSelected)
                    {
                        iTemp = tmp;
                        yIndex = i;
                    }
                }
            }
            if (iTemp == null)
                return;
            this.iTemplate.Clear();
            for (int i = 0; i < count; i++)
            {
                ShapeTemplate temp = null;
                Border_Content BorderContent = null;
                if (this.main_layout.Children[i] is Border_Content)
                {
                    temp = new Data.Store.ShapeTemplate();
                    BorderContent = this.main_layout.Children[i] as Border_Content;
                    temp.Parse(BorderContent);
                    this.iTemplate.Add(temp);
                }
            }
            iTemp.Data = this.iTemplate;
        }

        private void Load_Template()
        {
            return;
            this.iTemplate = new Template();
            this.iTemplate = this.iTemplate.loadXml() as Template;
            int count = this.iTemplate.Count;
            for (int i = 0; i < count; i++)
            {
                Alta_LED.Data.Store.ShapeTemplate template = new ShapeTemplate();
                template = this.iTemplate.Children[i];
                Border_Content content = template.Parse();
                content.selectEvent += content_selectEvent;
                this.main_layout.Children.Add(content);
            }
        }

        private void Grid_Content_LoadTemplate(object sender, Template e)
        {
            List<Border_Content> list_BorderContent = e.Parse();
            int count = list_BorderContent.Count;
            for (int i = 0; i < count; i++)
            {
                list_BorderContent[i].selectEvent += content_selectEvent;
                this.main_layout.Children.Add(list_BorderContent[i]);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Matrix_Template matrix = new Matrix_Template();
            int countTab = this.BankContainer.Items.Count - 1;
            for (int i = 0; i < countTab; i++)
            {
                TabItem Tab = this.BankContainer.Items[i] as TabItem;
                Grid_Content content = Tab.Content as Grid_Content;
                matrix.Add(content.Parse());
            }
            App.dataResource = matrix;
        }

        private void Clear_Layout(object sender, RoutedEventArgs e)
        {
            this.main_layout.Children.Clear();
        }

        private void delete_Layout(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.isSelected)
                    {
                        this.main_layout.Children.Remove(tmp);
                        break;
                    }
                }
                else if (this.main_layout.Children[i] is alta_Screen)
                {
                    alta_Screen screen = this.main_layout.Children[i] as alta_Screen;
                    if (screen.isSelected)
                    {
                        this.main_layout.Children.Remove(screen);
                        break;
                    }
                }
            }
        }

        private void btn_permuted(object sender, RoutedEventArgs e)
        {
            String tmp = this.txt_height_layout.Text;
            this.txt_height_layout.Text = this.txt_width_layout.Text;
            this.txt_width_layout.Text = tmp;
            double num_tmp = this.Layout_Height;
            this.Layout_Height = this.Layout_Width;
            this.Layout_Width = num_tmp;  
        }

        private void Width_input(object sender, KeyEventArgs e)
        {
            String tmp = this.txt_width_layout.Text.Trim();
            if (tmp.isNumber())
            {
                this.Layout_Width = Convert.ToDouble(tmp);
            }
            else
            {
               MessageBox.Show("Giá trị nhập vào không đúng?");
               String tmp1= this.txt_width_layout.Text.Substring(0, tmp.Length - 1);
               if (tmp1.Length < 1)
                   tmp1 = "0";
               this.txt_width_layout.Text = tmp1;
            }
           
        }

        private void Height_Input(object sender, KeyEventArgs e)
        {
            String tmp = this.txt_height_layout.Text.Trim();
            if (tmp.isNumber())
            {
                this.Layout_Height = Convert.ToDouble(tmp);
            }
            else
            {
                MessageBox.Show("Giá trị nhập vào không đúng?");
                String tmp1 = this.txt_height_layout.Text.Substring(0, tmp.Length - 1);
                if (tmp1.Length < 1)
                    tmp1 = "0";
                this.txt_height_layout.Text = tmp1;
            }
           
        }

        private void Play_video_selected(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.MediaSource != null && tmp.isSelected)
                    {
                        tmp.ModePlayer = PlayerMode.Play;
                    }
                }
                else if (this.main_layout.Children[i] is alta_Screen)
                {
                    alta_Screen @alta_Screen = this.main_layout.Children[i] as alta_Screen;
                    if (alta_Screen.MediaSource != null && alta_Screen.isSelected)
                    {
                        alta_Screen.ModePlayer = PlayerMode.Play;
                    }
                }
            }
        }

        private void Change_zindex_desc(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.isSelected)
                    {
                        tmp.zIndex--;
                    }
                }
            }
        }

        private void Change_zindex_asc(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.isSelected)
                    {
                        tmp.zIndex++;
                    }
                }
            }
        }

        private void Change_Volume(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.isSelected)
                    {
                        tmp.isMute = !tmp.isMute;
                        if (!tmp.isMute)
                        {
                            this.btn_Volume.Content = "\uf028";
                        }
                        else
                        {
                            this.btn_Volume.Content = "\uf026";
                        }
                    }
                }
            }
        }
        double oldScale=100;
       
        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox cb_Scale = sender as ComboBox;
            String text = cb_Scale.Text;
            if (text.isNumber())
            {
                Double tmp = Convert.ToDouble(cb_Scale.Text);              
                this.Border_layout.ScaleLayout(tmp / this.oldScale, tmp / this.oldScale);
            }
           
        }

        private void Create_screen(object sender, RoutedEventArgs e)
        {
              int count = this.main_layout.Children.Count;
              bool flag = false;
              alta_Screen Screen = new alta_Screen();
              Screen.selectEvent += content_selectEvent;
              for (int i = 0; i < count; i++)
              {
                  if (this.main_layout.Children[i] is Border_Content)
                  {
                      Border_Content @Border_Content = this.main_layout.Children[i] as Border_Content;
                      if (Border_Content.isSelected)
                      {
                        
                          Screen.AddProperty(Border_Content.Property);
                          Screen.setPosition(Border_Content.getPosition());
                          flag = true;
                      }
                  }
              }
            if(flag)
              this.main_layout.Children.Add(Screen);
        }

        private void repeat_click(object sender, RoutedEventArgs e)
        {
            int count = this.main_layout.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.main_layout.Children[i] is Border_Content)
                {
                    Border_Content tmp = this.main_layout.Children[i] as Border_Content;
                    if (tmp.isSelected)
                    {
                        tmp.repeatMedia = !tmp.repeatMedia;
                        return;
                    }
                }
            }
            
        }

        
    }
}
