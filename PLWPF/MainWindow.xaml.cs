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


namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void AddGuestRequest_Click(object sender, RoutedEventArgs e)
        {
            AddGuestRequest adding = new AddGuestRequest();
            adding.ShowDialog();           
            
        }

        private void HostingUnitManage_Click(object sender, RoutedEventArgs e)
        {
            HostingUnitManage hostManage = new HostingUnitManage();
            hostManage.ShowDialog();

        }

        private void OrderManage_Click(object sender, RoutedEventArgs e)
        {
            OrderManage orderManage = new OrderManage();
            orderManage.ShowDialog();

        }

        private void owner(object sender, RoutedEventArgs e)
        {
            OwnerWindow window = new OwnerWindow();
            window.ShowDialog();
        }
    }
}
