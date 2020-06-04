using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface Idal
    {
        GuestRequest addGuestRequest(GuestRequest g);
        void updateGuestRequest(GuestRequest g);
        void addHostingUnit(HostingUnit hu);
        void deleteHostingUnit(HostingUnit hu);
        void updateHostingUnit(HostingUnit hu);
        void addOrder(GuestRequest g, List<HostingUnit> suites);
        void updateOrder(Order ord);
        List<HostingUnit> GetAllHostingUnits();
        List<GuestRequest> getAllGuests();
        List<Order> getAllOrder();
        List<BankBranch> GetAllBankBranch();
        bool didHaveNeme(string name);
        HostingUnit hostingUnitExist(object key);
    }
    public class FuctoryDal
    {
        private static Idal instance = null;
        public static Idal getDal()
        {
            if (instance == null)
                instance = new DAL_Xml_imp();
            return instance;
        }
    }
}
