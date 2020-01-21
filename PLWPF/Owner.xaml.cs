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
        private BL.BL_imp bl = BL.BL_imp.getBl();
        private Guest_Requests guest=new Guest_Requests();
        public Owner()
        {
            InitializeComponent();
            foreach (var guestrequest in bl.GetAllGuests())
            {
                Guest_Requests guest_ = new Guest_Requests();
                guest_.area = "" + (Area)guestrequest.Area;
                guest_.nofshim = (guestrequest.Children + guestrequest.Adults);
                guest_.start = guestrequest.EntryDate.ToString("dd/MM/yy");
                guest_.finish = guestrequest.ReleaseDate.ToString("dd/MM/yy");
                list.Add(guest_);
            }
            ListOfGuestRequest.Opacity = list.Any() ? 1 : 0;
            DataContext = list;
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
            var list =bl.groupGuestRequestByNumOfVacationer();
            CreateList(list);
        }

        private void ByArea(object sender, RoutedEventArgs e)
        {
            var byArea = bl.groupGuestRequestByArea();
            CreateList(byArea);
        }
        private void CreateList(Dictionary<int,List<GuestRequest>> guestRequestlist)
        {
            list.Clear();
            foreach (var item in guestRequestlist)
            {
                DataContext = item.Key;
                foreach (var guestrequest in item.Value)
                {
                    Guest_Requests guest_ = new Guest_Requests();
                    guest_.area =""+ (Area)guestrequest.Area;
                    guest_.nofshim = (guestrequest.Children + guestrequest.Adults);
                    guest_.start = guestrequest.EntryDate.ToString("dd/MM/yy");
                    guest_.finish = guestrequest.ReleaseDate.ToString("dd/MM/yy");
                    list.Add(guest_);
                }                
            }
           
            DataContext = list;
        }
        
    }
}
