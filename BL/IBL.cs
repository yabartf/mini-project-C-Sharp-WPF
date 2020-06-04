using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
namespace BL
{
    public interface IBL
    {
        void addGuestRequest(GuestRequest g);
        void updateGuestRequest(GuestRequest g);
        void addHostingUnit(HostingUnit hu);
        bool deleteHostingUnit(int inputKey);
        void updateHostingUnit(HostingUnit hu);
        void addOrder(GuestRequest g, List<HostingUnit> hu);
        void updateOrder(Order ord);
        List<HostingUnit> GetAllHostingUnits();
        List<GuestRequest> GetAllGuests();
        List<Order> getAllOrder();
        List<BankBranch> GetAllBankBranch();
        void UpdateAllOrders();        
        
        bool IsCommonDates(GuestRequest source, GuestRequest other);
        IEnumerable<GuestRequest> li();
        Dictionary<int, List<GuestRequest>> groupGuestRequestByArea();
        Dictionary<int, List<Order>> GroupOrdersByGuestRequest();
        Dictionary<int, List<Order>> GroupOrdersByStatus();
        Dictionary<int, List<Order>> GroupOrdersByHostingUnit();
        Dictionary<int, List<GuestRequest>> groupGuestRequestByNumOfVacationer();
        List<Host> groupOwnersBYNumOfHostingUnit();
        Dictionary<int, List<HostingUnit>> GroupHOstingUnitByPricePerNight();
        List<Order> OrdersOfHostingUnit(HostingUnit hu);
        Dictionary<int, List<HostingUnit>> groupHostingUnitByArea();
        List<Order> AllNotAddressed();
        List<HostingUnit> hostingUnitsByArea(int selectedArea);
        bool haveNeme(string name);
        
        List<HostingUnit> ownersHostingUnits(int id);
        HostingUnit hostingUnitByOrder(Order order);
        GuestRequest guestRequestByOrder(Order order);
        HostingUnit TheHostingUnitByKey(int key);
        HostingUnit TheHostingUnitByName(string name);
        HostingUnit hostingUnitExist(object key);
    }
    public class FuctoryBl
    {
        private static BL_imp instance = null;
        public static BL_imp getBl()
        {
            if (instance == null)
                instance = new BL_imp();
            return instance;
        }
    }
}
