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
using System.Collections.ObjectModel;
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Owner.xaml
    /// </summary>
    public partial class Owner : Window
    {
        private ObservableCollection<Guest_Requests> list = new ObservableCollection<Guest_Requests>();
        IBL bl = FuctoryBl.getBl();
        private Guest_Requests guest = new Guest_Requests();
        public Owner()
        {
            InitializeComponent();
            CreateListFromList(bl.GetAllGuests());
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        public class Guest_Requests
        {
            public string start, finish, area;
            public int nofshim;
            public override string ToString()
            {
                return string.Format("entery date: " + start + "    release date: " + finish + "    vacationers: " + nofshim + "    area: " + area);
            }
        }

        private void ByVacationer(object sender, RoutedEventArgs e)
        {
            var guestRequestDict = bl.groupGuestRequestByNumOfVacationer();
            CreateListFromDictionary(guestRequestDict);
        }

        private void ByArea(object sender, RoutedEventArgs e)
        {
            var guestRequestDict = bl.groupGuestRequestByArea();
            CreateListFromDictionary(guestRequestDict);
        }
        private void CreateListFromDictionary(Dictionary<int, List<GuestRequest>> guestRequestDict)
        {
            list.Clear();
            foreach (var item in guestRequestDict)
            {
                DataContext = item.Key;
                foreach (var guestrequest in item.Value)
                {
                    Guest_Requests guest_ = new Guest_Requests();
                    guest_.area = "" + (Area)guestrequest.Area;
                    guest_.nofshim = (guestrequest.Children + guestrequest.Adults);
                    guest_.start = guestrequest.EntryDate.ToString("dd/MM/yy");
                    guest_.finish = guestrequest.ReleaseDate.ToString("dd/MM/yy");
                    list.Add(guest_);
                }
            }
            if (list.Any())
            {
                noData.Visibility = Visibility.Collapsed;
                ListOfGuestRequest.Opacity = 1;
                DataContext = list;
            }
            else
            {
                ListOfGuestRequest.Opacity = 0;
                noData.Visibility = Visibility.Visible;
            }
        }
        private void CreateListFromList(List<GuestRequest> guestRequestlist)
        {
            list.Clear();
            foreach (var item in guestRequestlist)
            {
                Guest_Requests guest_ = new Guest_Requests();
                guest_.area = "" + (Area)item.Area;
                guest_.nofshim = (item.Children + item.Adults);
                guest_.start = item.EntryDate.ToString("dd/MM/yy");
                guest_.finish = item.ReleaseDate.ToString("dd/MM/yy");
                list.Add(guest_);
            }
            if (list.Any())
            {
                noData.Visibility = Visibility.Collapsed;
                ListOfGuestRequest.Opacity = 1;
                DataContext = list;
            }
            else
            {
                ListOfGuestRequest.Opacity = 0;
                noData.Visibility = Visibility.Visible;

            }
        }
    }
}
