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
    }
}
