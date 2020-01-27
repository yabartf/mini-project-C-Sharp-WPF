using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public class BL_imp : IBL
    {
        DAL_Xml_imp dal = DAL_Xml_imp.getDal_XML();
        private static BL_imp bl = null;
        public delegate void threadstart();

        Thread UpdateOrders;
        public void UpdateAllOrders()
        {
            DateTime d = DateTime.Now;
            d = d.AddDays(-30);
            foreach (var item in getAllOrder())
            {
                if (item.OrderDate.AddDays(30) < DateTime.Now && item.status == (int)Status.SentMail)
                {
                    item.status = (int)Status.Faild;
                    updateOrder(item);
                }
            }
            foreach (var item in GetAllHostingUnits())
            {
                item.Diary[d.Month, d.Day] = false;
                updateHostingUnit(item);
            }
            Thread.Sleep(86400000);
        }

        private BL_imp()
        {
            //  dal.restartLists();            
            new Thread(UpdateAllOrders);
        }

        public static BL_imp getBl()
        {
            if (bl == null)
                bl = new BL_imp();
            return bl;
        }

        /// <summary>
        /// check if duration > 1
        /// </summary>
        /// <param name="guestrequest"></param>
        /// <returns></returns>
        public bool legitDates(GuestRequest guestrequest)
        {
            try
            {
                DateTime from = DateTime.Now;
                from = from.AddDays(1);
                DateTime to = DateTime.Now;
                to = to.AddMonths(12);
                if (guestrequest.EntryDate < from ||
                    guestrequest.EntryDate > to ||
                    guestrequest.ReleaseDate > to)
                    throw new OurException("you can only order from tumorrow up to one year from now");
                TimeSpan t = guestrequest.ReleaseDate - guestrequest.EntryDate;
                if (t.Days < 1)
                    throw new OurException("douration is not legit");
                if (t.Days >= 1)
                    return true;
                return false;
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }
        public bool craditAuthorization(Host h, Order order)
        {
            if (h.CollectionClearance)
            {
                order.status = (int)Status.SentMail;
                return true;
            }
            return false;
        }

        /// <summary>
        /// look for hostung unit suit for geus request 
        /// </summary>
        /// <param name="guestrequest"></param>
        /// <returns></returns>
        public List<HostingUnit> suitble(GuestRequest guestrequest)
        {
            HostingUnit hostingunit = new HostingUnit();
            var hostingUnitList = from item in GetAllHostingUnits()
                                  where wantFilter(guestrequest, item).All(f => f(guestrequest, item)) && checkDates(guestrequest, item)
                                  select item;
            
            return hostingUnitList.ToList();
        }

        /// <summary>
        /// chck if the dates empty
        /// </summary>
        /// <param name="guestrequest"></param>
        /// <param name="hostingunit"></param>
        /// <returns></returns>
        public bool checkDates(GuestRequest guestrequest, HostingUnit hostingunit)
        {
            // check if all is proper
            for (DateTime tempDate = guestrequest.EntryDate;
                tempDate < guestrequest.ReleaseDate;
                tempDate = tempDate.AddDays(1))
            {
                if (hostingunit.Diary[tempDate.Month - 1, tempDate.Day - 1])
                    return false; // if once wrong instant return false.
            }

            return true;
        }
        /// <summary>
        /// adding guest and suit order
        /// </summary>
        /// <param name="g"></param>
        public void addGuestRequest(GuestRequest g)
        {
            try
            {
                if (checkGuestRequestDetails(g))
                {
                    List<HostingUnit> listHostings = suitble(g);
                    if (!listHostings.Any())
                        throw new OurException("אין יחידת אירוח מתאימה");
                    g = dal.addGuestRequest(g.Copy());
                    addOrder(g, listHostings);
                }
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
                checkHostingUnitDetails(hu);
                dal.addHostingUnit(hu.Copy());
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }


        public List<BankBranch> GetAllBankBranch()
        {
            return dal.GetAllBankBranch();
        }

        public List<GuestRequest> GetAllGuests()
        {
            try
            {
                return dal.getAllGuests();
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
                return dal.GetAllHostingUnits();
            }
            catch (OurException ex)
            {

                throw ex;
            }
        }

        public List<Order> getAllOrder()
        {

            try
            {
                return dal.getAllOrder();
            }
            catch (OurException ex)
            {

                throw ex;
            }
        }

        public void updateGuestRequest(GuestRequest guestrequest)
        {
            try
            {
                if (checkGuestRequestDetails(guestrequest))
                    dal.updateGuestRequest(guestrequest.Copy());
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public void updateHostingUnit(HostingUnit hu)
        {
            try
            {
                checkHostingUnitDetails(hu);
                dal.updateHostingUnit(hu.Copy());
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// check if the dates of guest rquests is commons
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsCommonDates(GuestRequest source, GuestRequest other)
        {
            if (source.EntryDate < other.EntryDate)
                return source.ReleaseDate > other.EntryDate;
            if (source.EntryDate > other.EntryDate)
                return source.EntryDate < other.ReleaseDate;
            return true;
        }
        /// <summary>
        /// update order and if other orders of this hosting unit cant to exist is closing they. 
        /// </summary>
        /// <param name="ord"></param>
        public void updateOrder(Order ord)
        {
            try
            {
                dal.updateOrder(ord.Copy());
                // check wich cant to exist from now.
                var faild_order = from item in hostingUnitByOrder(ord).orders
                                  where IsCommonDates(guestRequestByOrder(ord), guestRequestByOrder(item))
                                  select item;
                foreach (var item in faild_order)
                {
                    item.status = (int)Status.Faild;
                    dal.updateOrder(item.Copy());
                }
            }
            catch (OurException)
            {
                throw new OurException();
            }
        }

        public void addHostingUnit()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<GuestRequest> li()
        {
            var list = from x in dal.getAllGuests()
                       where x.Area == (int)Area.Jerusalem
                       select x;
            return list.ToList();
        }

        public Dictionary<int, List<GuestRequest>> groupGuestRequestByArea()
        {
            var gr = dal.getAllGuests().GroupBy(s => s.Area).ToDictionary(x => x.Key, x => x.ToList());
            return gr;
        }

        public Dictionary<int, List<GuestRequest>> groupGuestRequestByNumOfVacationer()
        {
            var gr = dal.getAllGuests().GroupBy(s => (s.Children + s.Adults))
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());
            return gr;
        }
        public List<Host> groupOwnersBYNumOfHostingUnit()
        {
            var owners = GetAllHostingUnits().GroupBy(x => x.Owner)
                .ToDictionary(x => x.Key).OrderBy(x => x.Value.Count()).ToDictionary(x => x.Key);
            return owners.Select(x => x.Key).ToList();
        }

        public Dictionary<int, List<HostingUnit>> groupHostingUnitByArea()
        {

            var hostingUnitByArea = dal.GetAllHostingUnits().GroupBy(s => s.area).ToDictionary(x => x.Key, x => x.ToList());
            return hostingUnitByArea;
        }

        /// <summary>
        /// search all orders that yet not addresseds.///
        /// </summary>
        /// <returns>list of orders </returns>
        public List<Order> AllNotAddressed()
        {
            List<Order> all = getAllOrder();
            var result =
                from Order res in all
                where res.status == (int)Status.NotAddressed
                select res;
            return (List<Order>)result.Copy();
        }
        public List<HostingUnit> hostingUnitsByArea(int selectedArea)
        {
            var hostingUnitsByArea = dal.GetAllHostingUnits()
                .Where(s => s.area == selectedArea)
                .OrderBy(s => s.Children + s.Adults);
            return hostingUnitsByArea.ToList();

        }
        /// <summary>
        /// check if specific have specific name in the hosts 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>bool</returns>
        public bool haveNeme(string name)
        {
            return dal.didHaveNeme(name.Copy());
        }

        List<Func<GuestRequest, HostingUnit, bool>> wantFilter(GuestRequest guestrequest, HostingUnit hostingunit)
        {
            var Filter = new List<Func<GuestRequest, HostingUnit, bool>>();
            Filter.Add((GuestRequest, HostingUnit) => hostingunit.Adults >= guestrequest.Adults);
            Filter.Add((GuestRequest, HostingUnit) => hostingunit.Children >= guestrequest.Children);
            Filter.Add((GuestRequest, HostingUnit) => hostingunit.Type == guestrequest.Type);
            Filter.Add((GuestRequest, HostingUnit) => hostingunit.Meals >= guestrequest.Meals);
            Filter.Add((GuestRequest, HostingUnit) =>
                guestrequest.Area == 1 ? true : guestrequest.Area == hostingunit.area);
            if (guestrequest.spa == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.spa);
            else if (guestrequest.spa == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.spa);
            if (guestrequest.airCondition == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.airCondition);
            else if (guestrequest.airCondition == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.airCondition);
            if (guestrequest.ChildrensAttractions == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.ChilldrensAttractions);
            else if (guestrequest.ChildrensAttractions == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.ChilldrensAttractions);
            if (guestrequest.flatTv == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.flatTv);
            else if (guestrequest.flatTv == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.flatTv);
            if (guestrequest.Garden == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.Garden);
            else if (guestrequest.Garden == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.Garden);
            if (guestrequest.Jacuzzy == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.Jacuzzy);
            else if (guestrequest.Jacuzzy == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.Jacuzzy);
            if (guestrequest.Pool == (int)Necessity.Interested)
                Filter.Add((GuestRequest, HostingUnit) => hostingunit.Pool);
            else if (guestrequest.Pool == (int)Necessity.NotInterested)
                Filter.Add((GuestRequest, HostingUnit) => !hostingunit.Pool);
            return Filter;
        }

        public bool deleteHostingUnit(int inputKey)
        {
            try
            {
                foreach (var item in dal.GetAllHostingUnits())
                    if (item.hostingUnitKey == inputKey)
                    {
                        dal.deleteHostingUnit(item.Copy());
                        return true;
                    }
                return false;
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public List<HostingUnit> ownersHostingUnits(int id)
        {
            try
            {
                var ownersHostingUnitsList = from ownersHostingUnit in GetAllHostingUnits()
                                             where ownersHostingUnit.Owner.HostId == id
                                             select ownersHostingUnit;
                if (ownersHostingUnitsList.Count() == 0)
                    throw new OurException("you have ni hosting units. maybe you wrote the wrong id");
                return ownersHostingUnitsList.ToList().Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HostingUnit hostingUnitByOrder(Order order)
        {
            try
            {
                foreach (var item in GetAllHostingUnits())
                {
                    if (item.hostingUnitKey == order.HostingUnitKey)
                        return item.Copy();
                }
                throw new OurException();
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public GuestRequest guestRequestByOrder(Order order)
        {
            try
            {
                foreach (var item in GetAllGuests())
                {
                    if (item.GuestRequestKey == order.GuestRequestKey)
                        return item.Copy();
                }
                throw new OurException();
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }
        public HostingUnit TheHostingUnitByKey(int key)
        {
            try
            {
                foreach (var item in GetAllHostingUnits())
                {
                    if (item.hostingUnitKey == key)
                        return item;
                }
                return null;
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }
        public HostingUnit TheHostingUnitByName(string name)
        {
            try
            {
                foreach (var item in GetAllHostingUnits())
                {
                    if (item.HostingUnitName == name)
                        return item.Copy();
                }
                return null;
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }
        public HostingUnit hostingUnitExist(object key)
        {
            try
            {
                return dal.hostingUnitExist(key);
            }
            catch (OurException ex)
            {
                throw ex;
            }
        }

        public void addOrder(GuestRequest g, List<HostingUnit> hu)
        {
            dal.addOrder(g.Copy(), hu.Copy());
        }
        public bool checkHostingUnitDetails(HostingUnit hostingUnit)
        {
            if (hostingUnit.Children < 0 || hostingUnit.Adults < 0)
                throw new OurException("מספר ילדים ומבוגרים חייב להיות גדול מ0");
            if (hostingUnit.Children == 0 && hostingUnit.Adults == 0)
                throw new OurException("מספר הנופשים חייב להיות גדול מ0");
            if (hostingUnit.area < 1 || hostingUnit.area > 5)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            if (hostingUnit.Type < 1 || hostingUnit.Type > 5)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            if (hostingUnit.Meals != 0 && hostingUnit.Meals != 2 && hostingUnit.Meals != 3)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            if (hostingUnit.pricePerNight <= 0)
                throw new OurException("מחיר חייב להיות גדול מ0");
            if (!checkFhoneNumber(hostingUnit.Owner.FhoneNumber))
                throw new OurException("מספר טלפון לא תקין");
            if (!checkName(hostingUnit.Owner.PrivateName) || !checkName(hostingUnit.Owner.FamilyName))
                throw new OurException("שם חייב להיות מורכב מאותיות בלבד");
            if (!checkName(hostingUnit.city))
                throw new OurException("שם עיר חייב להיות מורכב מאותיות בלבד");
            if (!checkName(hostingUnit.Owner.BankBranchDetails.BranchCity))
                throw new OurException("שם עיר חייב להיות מורכב מאותיות בלבד");
            //checkBankDitails(hostingUnit);
            return true;
        }
        private bool checkFhoneNumber(int fNumber)
        {
            string fhoneNumber = "" + fNumber;
            if (fhoneNumber.Length == 9)
                return true;
            return false;
        }
        private bool checkName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                if ((name[i] > 122 || name[i] < 65) || (name[i] > 90 && name[i] < 97))
                    return false;
            }
            return true;
        }
        private bool checkGuestRequestDetails(GuestRequest guestRequest)
        {
            legitDates(guestRequest);
            if (guestRequest.Children < 0 || guestRequest.Adults < 0)
                throw new OurException("מספר מבוגרים וילדים לא יכול להיות שלילי");
            if (guestRequest.Children == 0 && guestRequest.Adults == 0)
                throw new OurException("מספר הנופשים חייב להיות גדול מ0");
            checkNecessityEnum(guestRequest.airCondition);
            if (guestRequest.Area < 1 || guestRequest.Area > 5)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            checkNecessityEnum(guestRequest.ChildrensAttractions);
            checkNecessityEnum(guestRequest.flatTv);
            checkNecessityEnum(guestRequest.Jacuzzy);
            checkNecessityEnum(guestRequest.Pool);
            checkNecessityEnum(guestRequest.Garden);
            checkNecessityEnum(guestRequest.spa);
            if (guestRequest.status < 1 && guestRequest.status > 5)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            if (guestRequest.Type < 1 && guestRequest.Type > 5)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 5");
            if (!checkName(guestRequest.PrivateName) || !checkName(guestRequest.FamilyName))
                throw new OurException("שם חייב להיות מורכב מאותיות בלבד");
            return true;
        }
        private void checkNecessityEnum(int neecessity)
        {
            if (neecessity < 1 || neecessity > 3)
                throw new OurException("מספר לא תקין, יכול לקבל ערכים מ1 עד 3");
        }
        //private void checkBankDitails(HostingUnit hostingUnit)//בדיקה של תנאי הבנק
        //{
        //    if(!dal.GetAllBankBranch().Exists(x=>x.BankNumber==hostingUnit.Owner.BankBranchDetails.BankNumber
        //    &&x.BankName==hostingUnit.Owner.BankBranchDetails.BankName))
        //}
    }
}


