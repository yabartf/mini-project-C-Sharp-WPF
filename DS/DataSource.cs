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
        static DataSource() { }
        public static List<HostingUnit> allHostingUnits = new List<HostingUnit>();
        public static List<GuestRequest> allGuests = new List<GuestRequest>();
        public static List<Order> allOrders = new List<Order>();
        public static List<int> Passwords = new List<int>();
        ////public static void restart_order()
        ////{
        ////    for (int i = 0; i < 10; i++)
        ////    {
        ////        Order ord = new Order();
        ////        ord.OrderKey = Configuration.OrderKey++;
        ////        ord.HostingUnitKey = allHostingUnits[i].hostingUnitKey;
        ////        ord.GuestRequestKey = allGuests[i].GuestRequestKey;
        ////        allOrders.Add(ord);
        ////    }

        ////}
        //public static void restart_hosting_unit()
        //{
        //    int te = rand.Next(10, 20);
        //    for (int i = 0; i < te; i++)
        //    {
        //        int days = rand.Next(100);
        //        HostingUnit temp = new HostingUnit();
        //        for (int j = 0; j < days; j++)
        //        {
        //            temp.Diary[rand.Next(11), rand.Next(0, 30)] = true;
        //        }
        //        temp.HostingUnitName = "hamood" + (i * 71);
        //        temp.hostingUnitKey = Configuration.hostingUnitKey++;
        //        allHostingUnits.Add(temp);
        //    }
        //}
        public static void CreateRandomGuestRequest()
        {
            for (int i = 0; i < rand.Next(10, 15); i++)
            {
                GuestRequest gs = new GuestRequest();
                int temp;
                foreach (var item in gs.varible)
                {
                    temp = item;
                    temp = rand.Next(3);
                }
                gs.PrivateName = "mustafa" + (232 * i);
                gs.FamilyName = "dirany" + (100 * i);
                gs.MailAddress = "musd@g.jct.ac.il" + (20 * i);
                DateTime tempDate = new DateTime(2020, 1, 1);
                int tempRand = rand.Next(355);   //adding randomaly number of days.
                gs.Adults = rand.Next(4);
                gs.Children = rand.Next(4);
                gs.Area = rand.Next(1, 5);
                gs.EntryDate = tempDate.AddDays(tempRand);
                gs.ReleaseDate = tempDate.AddDays(tempRand + rand.Next(2, 10));
                gs.GuestRequestKey = Configuration.GuestRequestKey++;
                allGuests.Add(gs);

            }
        }

    }

}
