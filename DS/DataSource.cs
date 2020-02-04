using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    static public class DataSource
    {


        static Random rand = new Random();
        static DataSource() { allHostingUnits.Add(new HostingUnit()); allGuests.Add(new GuestRequest()); }
        public static List<HostingUnit> allHostingUnits = new List<HostingUnit>();
        public static List<GuestRequest> allGuests = new List<GuestRequest>();
        public static List<Order> allOrders = new List<Order>();
        public static List<int> Passwords = new List<int>();

    }

}
