using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AllOrders.xaml
    /// </summary>
    public partial class AllOrders : Window
    {
        private ObservableCollection<Order> listByGuest = new ObservableCollection<Order>();
        private ObservableCollection<Order> listByStatus = new ObservableCollection<Order>();
        private ObservableCollection<Order> listByHosting = new ObservableCollection<Order>();
        IBL bl = FuctoryBl.getBl();
        public AllOrders()
        {
            InitializeComponent();
            var OrderDict = bl.GroupOrdersByGuestRequest();
            foreach (var item in OrderDict)
            {
                foreach (var OneOrder in item.Value)
                {
                    listByGuest.Add(OneOrder);
                }
            }
            if (listByGuest.Any())
                ListOfOrder.Opacity = 1;
            else
            {
                ListOfOrder.Opacity = 0;
                MessageBox.Show("אין נתונים להצגה");

            }
            DataContext = listByGuest;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (combo.SelectedIndex)
            {
                case 0:
                    
                    var ByStatus = bl.GroupOrdersByStatus();
                    foreach (var item in ByStatus)
                    {
                        foreach (var OneOrder in item.Value)
                        {
                            listByStatus.Add(OneOrder);
                        }
                    }
                    if (listByStatus.Any())
                        ListOfOrder.Opacity = 1;
                    else
                    {
                        ListOfOrder.Opacity = 0;
                        MessageBox.Show("אין נתונים להצגה");
                    }
                    

                    DataContext = listByStatus;
                    break;
                case 1:
                    listByGuest = new ObservableCollection<Order>();
                    var ByGuest = bl.GroupOrdersByGuestRequest();
                    foreach (var item in ByGuest)
                    {
                        foreach (var OneOrder in item.Value)
                        {
                            listByGuest.Add(OneOrder);
                        }
                    }
                    if (listByGuest.Any())
                        ListOfOrder.Opacity = 1;
                    else
                    {
                        ListOfOrder.Opacity = 0;
                        MessageBox.Show("אין נתונים להצגה");

                    }
                    DataContext = listByGuest;
                    break;
                case 2:                    
                    var ByHosting = bl.GroupOrdersByHostingUnit();
                    foreach (var item in ByHosting)
                    {
                        foreach (var OneOrder in item.Value)
                        {
                            listByHosting.Add(OneOrder);
                        }
                    }
                    if (listByHosting.Any())
                        ListOfOrder.Opacity = 1;
                    else
                    {
                        ListOfOrder.Opacity = 0;
                        MessageBox.Show("אין נתונים להצגה");

                    }
                    DataContext = listByHosting;
                    break;
              
            }
        }
    }
}
