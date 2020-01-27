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
        bool noOrders(HostingUnit hu);
        List<HostingUnit> GetAllHostingUnits();
        List<GuestRequest> getAllGuests();
        List<Order> getAllOrder();
        List<BankBranch> GetAllBankBranch();



    }




}
