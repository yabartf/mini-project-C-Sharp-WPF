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
        static public bool PasTrue = false;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            PasTrue = false;
            OrderManage orderManage = new OrderManage();
            Password pas = new Password();
            pas.ShowDialog();
            if (PasTrue)
                orderManage.ShowDialog();
            PasTrue = false;
        }

        private void owner(object sender, RoutedEventArgs e)
        {
            PasTrue = false;
            OwnerWindow window = new OwnerWindow();
            Password pas = new Password();
            pas.ShowDialog();
            if (PasTrue)
                window.ShowDialog();
            PasTrue = false;
        }
    }
}
