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
using BL;
using BE;
using System.Collections.ObjectModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AllGuestRequests.xaml
    /// </summary>
    public partial class AllHostingUnit : Window
    {
        BL_imp bl = BL_imp.getBl();
        private ObservableCollection<HostingUnit> list = new ObservableCollection<HostingUnit>();
        public AllHostingUnit()
        {
            InitializeComponent();
            var HostingUnitDictionry = bl.groupHostingUnitByArea();
                list.Clear();
                foreach (var item in HostingUnitDictionry)
                {
                    DataContext = item.Key;
                    foreach (var hosting in item.Value)
                    {
                    list.Add(hosting);
                    }
                }
                ListOfGuestRequest.Opacity = list.Any() ? 1 : 0;
                DataContext = list;
            }
        }
    }

