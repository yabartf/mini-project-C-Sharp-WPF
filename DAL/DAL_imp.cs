using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
namespace DAL
{
    public class DAL_imp : Idal
    {
        private DAL_imp()
        { }
        private static DAL_imp dal=null;
        public static DAL_imp GetDal()
        {
            if (dal == null)
                dal = new DAL_imp();
            return dal;

        }
        public HostingUnit getHostingUnit(HostingUnit hu)
        {
            try
            {
                foreach (var item in DataSource.allHostingUnits)
                {
                    if (item.hostingUnitKey == hu.hostingUnitKey)
                        return item.Copy();
                }
                throw new OurException("this hosting unit not found.");
            }
            catch (OurException ex)
            {
                throw ex;
            }                       
        }
        public GuestRequest addGuestRequest(GuestRequest g)
        {
            try
            {
                foreach (var item in DataSource.allGuests)
                {
                    if (g.GuestRequestKey == item.GuestRequestKey)
                    {
                        throw new OurException("this guest request is all ready in exist.");
                    }
                }
                g.GuestRequestKey = Configuration.GuestRequestKey++;
                DataSource.allGuests.Add(g);
                return g.Copy();
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public void addHostingUnit(HostingUnit hu)
        {
            try
            {
                foreach (var item in DataSource.allHostingUnits)
                {
                    
                }
                hu.hostingUnitKey = Configuration.hostingUnitKey++;
                DataSource.allHostingUnits.Add(hu);
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public void addOrder(GuestRequest g, List<HostingUnit>suites)
        {
            foreach (var item in suites)
            {
                Order ord = new Order();
                ord.GuestRequestKey = g.GuestRequestKey;
                ord.OrderKey = Configuration.OrderKey++;
                ord.HostingUnitKey = item.hostingUnitKey;
                ord.status = (int)Status.NotAddressed;
                item.orders.Add(ord);
                DataSource.allOrders.Add(ord);
                updateHostingUnit(item);
            }
        }

        public void deleteHostingUnit(HostingUnit hu)
        {
            try
            {
                foreach (var item in DataSource.allHostingUnits)
                {
                    if (item.hostingUnitKey == hu.hostingUnitKey)
                    {
                        DataSource.allHostingUnits.Remove(item);
                        return;
                    }
                }
                throw new OurException("this hosting unit not found!");
            }
            catch (OurException ex)
            {
                throw ex;
            }            
        }
        public void SetDairy(Order ord)
        {
            HostingUnit hoting = hostingUnitExist(ord.HostingUnitKey);
            GuestRequest gu = guestRequestExist(ord.GuestRequestKey);
            for (DateTime i = gu.EntryDate; i < gu.ReleaseDate; i=i.AddDays(1))
            {
                hoting.Diary[i.Month - 1, i.Day - 1] = true;
            }
            updateHostingUnit(hoting);
        }
        /// <summary>
        /// restart bank for program.
        /// </summary>
        /// <returns></returns>
        public List<BankBranch> GetAllBankBranch()
        {

            string[] bank_name = new string[5] { "mizrahi", "poalim", "yahav", "leumi", "discont" };
            string[] bank_address = new string[5] { "ben yehuda 21", "havad valeumi 21", "hapalmach 1", "yafo 132", "hanevi'im 541" };
            List<BankBranch> banks = new List<BankBranch>().Copy();

            for (int i = 0; i < 5; i++)
            {
                BankBranch bank = new BankBranch();
                bank.BankName = bank_name[i];
                bank.BranchNumber = (i * 10) + 11;
                bank.BranchAddress = bank_address[i];
                bank.BranchCity = "jerusalem";
                bank.BankNumber = Configuration.BankNumber++;
                banks.Add(bank);
            }
            return banks.Copy();
            throw new NotImplementedException();
        }

        public List<GuestRequest> getAllGuests()
        {
            try
            {
                
                return DataSource.allGuests.Copy();
            }
            catch (OurException ex)
            {
                throw ex;
            }            
        }

        public List<HostingUnit> GetAllHostingUnits()
        {
            try
            {
                return DataSource.allHostingUnits.Copy();
            }
            catch (OurException ex)
            {

                throw ex;
            }            
        }

        public List<Order> getAllOrder()
        {
            
            return DataSource.allOrders.Copy();
        }
        

        public void updateGuestRequest(GuestRequest guestrequest)
        {
            GuestRequest temp = new GuestRequest();
            temp.GuestRequestKey = 0;
            foreach (var item in DataSource.allGuests)
                if (guestrequest.GuestRequestKey == item.GuestRequestKey)
                {
                    DataSource.allGuests.Add(guestrequest);
                    return;
                }
            throw new OurException();
        }

        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            foreach (var item in GetAllHostingUnits())
            {
                if(item.hostingUnitKey==hostingUnit.hostingUnitKey)
                {
                    deleteHostingUnit(item);
                    DataSource.allHostingUnits.Add(hostingUnit);
                    return;
                }
            }
            addHostingUnit(hostingUnit);          
        }

        public void updateOrder(Order order)
        {
            try
            {
                foreach (var item in DataSource.allOrders)
                {                    
                    if (item.OrderKey == order.OrderKey)
                    {
                        //if() futre if complate!
                        item.status = order.status;
                        if (order.status == (int)Status.Complate|| order.status == (int)Status.SentMail)
                            SetDairy(order);
                        DataSource.allOrders.Remove(item);
                        DataSource.allOrders.Add(order);
                        hostingUnitExist(order.HostingUnitKey).orders.Remove(item);
                        hostingUnitExist(order.HostingUnitKey).orders.Add(order);
                    }
                    return;
                }
                throw new OurException("this order not found.");
            }
            catch (OurException ex)
            {
                throw ex;
            }            
        }
        public bool noOrders(HostingUnit hu)
        {
            if (!hu.orders.Any())
                return true;
            return false;
        }
        /// <summary>
        /// look for hosting unit by name owner
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool didHaveNeme(string name)
        {
            var answer =
                from hu in DataSource.allHostingUnits
                where hu.Owner.PrivateName == name
                select hu;
            foreach (var item in answer)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// check if hosting unit excist
        /// </summary>
        /// <param name="hostingUnit"></param>
        /// <returns></returns>
        public HostingUnit hostingUnitExist(object key)
        {
            int intKey;
            bool isInt = int.TryParse(""+key, out intKey);
            if(isInt)
            {
                foreach (var item in GetAllHostingUnits())
                {
                    if (intKey == item.hostingUnitKey)
                        return item.Copy();
                }

                return null;
            }
            else
            {
                foreach (var item in GetAllHostingUnits())
                {
                    if ((string)key == item.HostingUnitName)
                        return item.Copy();
                }

                return null;

            }
        }
        public GuestRequest guestRequestExist(int key)
        {
            foreach (var item in getAllGuests())
            {
                if (key == item.GuestRequestKey)
                    return item.Copy();
            }
            return null;
        }
        public Order orderExist(int key)
        {
            foreach (var item in getAllOrder())
            {
                if (key == item.OrderKey)
                    return item.Copy();
            }
            return null;
            
        }
        public void restartLists()
        {
           DataSource.CreateRandomGuestRequest();
        }
    }
}
