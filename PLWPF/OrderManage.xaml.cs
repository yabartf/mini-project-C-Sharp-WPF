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
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderManage.xaml
    /// </summary>
    public partial class OrderManage : Window
    {
       
        private BL.BL_imp bl = BL.BL_imp.getBl();
        private HostingUnit cuurntHostingUnit = new HostingUnit();
        private bool IsPressed=false , IsString = false;
        Button[] button;
        public OrderManage()
        {
            InitializeComponent();
            WindowOfOrder.Opacity = 0;
        }
        /// <summary>
        /// print all order of the hosting unit. all ListBoxItem is `TextBox and Button.
        /// </summary>
        private void PrintOrder()
        {
            int i = 0;
            ListBoxItem[] boxItem = new ListBoxItem[cuurntHostingUnit.orders.Count];
            foreach (var item in cuurntHostingUnit.orders)
            {           
                    GuestRequest gu = bl.guestRequestByOrder(item);
                    Canvas canvas = new Canvas();                   
                    canvas.Height = 23;
                    boxItem[i] = new ListBoxItem();
                    boxItem[i].Height = 28;
                    boxItem[i].Name = "a" + i;
                    TextBlock textBlock = new TextBlock();
                    Thickness thickness = new Thickness(150, 0, 0, 0);
                    textBlock.Margin = thickness;
                    textBlock.Background = Brushes.Beige;
                    textBlock.Width = 700;
                    textBlock.Height = 23;
                    textBlock.FontSize = 18;
                    textBlock.Foreground = Brushes.Brown;
                    textBlock.Text = gu.EntryDate.ToString("dd/MM/yyyy") + ":תאריך התחלה " + gu.ReleaseDate.ToString("dd/MM/yyyy") + " תאריך סיום " ;
                    canvas.Children.Add(textBlock);
                    thickness = new Thickness(0, 0, 700, 0);
                    button[i] = new Button();                    
                    button[i].HorizontalAlignment = HorizontalAlignment.Left;
                    button[i].Content = "אשר";
                    button[i].Click += OrderChoosed;                    
                    button[i].Height = 23;
                    button[i].Width = 110;                                        
                    button[i].Margin = thickness;
                if (item.status == (int)Status.Complate || item.status == (int)Status.Faild || item.status == (int)Status.SentMail)
                {
                    button[i].IsEnabled = false;
                    button[i].Background = Brushes.Red;
                    button[i].Foreground = Brushes.Black;
                    button[i].Content = "סגור";
                }
                canvas.Children.Add(button[i]);
                    boxItem[i].Content = canvas;
                    WindowOfOrder.Items.Add(boxItem[i]);                
                    i++;
                WindowOfOrder.Opacity = 1;
            }
        }        
        private void OrderChoosed(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = 0;
                for (int i = 0; i < cuurntHostingUnit.orders.Count; i++)
                {
                    if (button[i].IsPressed)
                        index = i;
                }
                cuurntHostingUnit.orders[index].status = (int)Status.Complate;
                bl.updateOrder(cuurntHostingUnit.orders[index]);
                
                /// need to send mail to the client by thread///
                MessageBox.Show("נשלח מייל ללקוח");
                PrintOrder();
            }
            catch (OurException ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void IsKey(object sender, KeyEventArgs e)
        {            
            if (e.Key <= Key.Z && e.Key >= Key.A)
            {
                IsString = true;
                e.Handled = false;
            }
        }        
        private void EnterHostingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                cuurntHostingUnit = IsString ? bl.TheHostingUnitByName(HostingKey.Text) : bl.TheHostingUnitByKey(int.Parse(HostingKey.Text));
                if (cuurntHostingUnit == null)
                    throw new OurException();
                button = new Button[cuurntHostingUnit.orders.Count];
                IsPressed = true;
                
            }
            catch (OurException)
            {
                MessageBox.Show("!הפרטים אינם נכונים");
                IsPressed = false;
            }
            if(IsPressed)
            PrintOrder();
        }
    }
}
