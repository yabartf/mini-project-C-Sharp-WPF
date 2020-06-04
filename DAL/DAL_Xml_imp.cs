using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters;
using System.IO;
using System.Net;
using System.Threading;

namespace DAL

{
    public class DAL_Xml_imp : Idal
    {
        private string OrdersPath = @"Orders.xml";
        private string GuestRequestsPath = @"guestRequests.xml";
        private string hostingUnitsPath = @"hostingUnits.xml";
        private string configPath = @"config.xml";
        XElement orderRoot = new XElement("orders");
        //XDocument DocorderRoot = XDocument.Load("Orders.xml");
        XElement hostingsRoot = new XElement("hosting_uints");
        XElement guestRequestRoot = new XElement("guest_request");
        XElement configRoot = new XElement("gonfigs");
        private static DAL_Xml_imp instance = null;
        List<BankBranch> bankBranches = new List<BankBranch>();
        public static DAL_Xml_imp getDal_XML()
        {
            if (instance == null)
                instance = new DAL_Xml_imp();
            return instance;
        }
        private DAL_Xml_imp()
        {
            if (!File.Exists(OrdersPath))
            {
                orderRoot = new XElement("orders");
                orderRoot.Save(OrdersPath);
            }
            else
            orderRoot = XElement.Load(OrdersPath);
            if (!File.Exists(GuestRequestsPath))
            {
                guestRequestRoot = new XElement("guest_request");
                guestRequestRoot.Save(GuestRequestsPath);
            }
            if (!File.Exists(hostingUnitsPath))
            {
                hostingsRoot = new XElement("hosting_uints");
                hostingsRoot.Save(hostingUnitsPath);
            }
            // saveListToXML(DataSource.allHostingUnits, hostingUnitsPath);

            if (!File.Exists(configPath))
            {
                configRoot = new XElement("configs");
                XElement hosKey = new XElement("hostingUnitKey", Configuration.hostingUnitKey);
                XElement gueKey = new XElement("GuestRequestKey", Configuration.hostingUnitKey);
                XElement ordKey = new XElement("OrderKey", Configuration.OrderKey);
                XElement com = new XElement("commissionFee", Configuration.commissionFee);
                configRoot.Add(hosKey, gueKey, ordKey, com);
                configRoot.Save(configPath);

                configRoot.Save(configPath);
            }
            else
            {
                configRoot = XElement.Load(configPath);
                updateCofingortion();
            }
            loadFromXml();
            new Thread(downloadBanks).Start();
            
        }
        #region Order Functions.
        public void addOrder(GuestRequest g, List<HostingUnit> suites)
        {
            
            foreach (var item in suites)
            {
                Order ord = new Order();
                ord.GuestRequestKey = g.GuestRequestKey;
                ord.OrderKey = Configuration.OrderKey++;
                configRoot.Element("OrderKey").Value = "" + Configuration.OrderKey;
                configRoot.Save(configPath);
                ord.HostingUnitKey = item.hostingUnitKey;
                ord.status = (int)Status.NotAddressed;
                item.orders.Add(ord);
                updateHostingUnit(item);
                XElement HosKey = new XElement("hostingUnitKey", ord.HostingUnitKey);
                XElement GueKey = new XElement("guestRequestKey", g.GuestRequestKey);
                XElement ordKey = new XElement("orderKey", ord.OrderKey);
                XElement stat = new XElement("status", ord.status);
                XElement creDate = new XElement("createDate", DateTime.Now);
                XElement ordDate = new XElement("orderDate", ord.OrderDate);
                XElement order = new XElement("order", HosKey, GueKey, ordKey, stat, creDate, ordDate);
                orderRoot.Add(order);
                DataSource.allOrders.Add(ord);
            }
            orderRoot.Save(OrdersPath);
        }

        public void updateOrder(Order ord)
        {
            try
            {
                Order old = DataSource.allOrders.Where(x => x.OrderKey == ord.OrderKey).FirstOrDefault();
                if (old == null)
                    throw new OurException();
                DataSource.allOrders.Remove(old);
                DataSource.allOrders.Add(ord);
                hostingUnitExist(ord.HostingUnitKey).orders.Find(x => x.OrderKey == ord.OrderKey).status = ord.status;
                if (ord.status == (int)Status.Complate || ord.status == (int)Status.SentMail && old.status==(int)Status.NotAddressed)
                    SetDairy(ord);
                foreach (var order in orderRoot.Elements())
                {
                    if(order.Element("orderKey").Value==""+ord.OrderKey)
                    {

                        order.Element("status").Value = "" + ord.status;
                        if (ord.status == (int)Status.SentMail || ord.status == (int)Status.Complate && old.status == (int)Status.NotAddressed)
                            order.Element("orderDate").Value = DateTime.Now.ToString();
                        
                    }
                }
               
                orderRoot.Save(OrdersPath);

            }
            catch (OurException)
            {

                throw new OurException("the order not found");
            }

        }

        #endregion

        #region Hosting unit Funcions.
        public void addHostingUnit(HostingUnit hu)
        {
            try
            {
                HostingUnit item = (from p in DataSource.allHostingUnits
                                    where p == hu
                                    select p).FirstOrDefault();
                if (item != null)
                    throw new OurException();
                else
                {
                    hu.hostingUnitKey = Configuration.hostingUnitKey++;
                    configRoot.Element("hostingUnitKey").Value = Configuration.hostingUnitKey.ToString();                    
                    DataSource.allHostingUnits.Add(hu);
                    saveListToXML(DataSource.allHostingUnits, hostingUnitsPath);
                }
            }
            catch (OurException)
            {

                throw new OurException("this hosting unit not found");
            }

        }

        public void deleteHostingUnit(HostingUnit hu)
        {
            try
            {
                HostingUnit huToDelet = DataSource.allHostingUnits.FirstOrDefault(x => x.hostingUnitKey == hu.hostingUnitKey);
                if(huToDelet==null)
                    throw new OurException();
                DataSource.allHostingUnits.Remove(huToDelet);
                saveListToXML(DataSource.allHostingUnits, hostingUnitsPath);
            }
            catch (OurException)
            {

                throw new OurException("this hosting unit not found");
            }

        }

        public void updateHostingUnit(HostingUnit hu)
        {
            try
            {
                HostingUnit item = (from p in DataSource.allHostingUnits
                                    where p.hostingUnitKey == hu.hostingUnitKey
                                    select p).FirstOrDefault();
                if (item == null)
                    throw new OurException();
                else
                {
                    DataSource.allHostingUnits.Remove(item);
                    DataSource.allHostingUnits.Add(hu);
                    saveListToXML(DataSource.allHostingUnits, hostingUnitsPath);
                }
            }
            catch (OurException)
            {

                throw new OurException("this hosting unit not found");
            }

        }

        #endregion

        #region Guest requests Functions.
        public GuestRequest addGuestRequest(GuestRequest g)
        {
            try
            {
                foreach (var item in DataSource.allGuests)
                {
                    if (g== item)
                    {
                        throw new OurException("this guest request is all ready in exist.");
                    }
                }
                g.GuestRequestKey = Configuration.GuestRequestKey++;
                configRoot.Element("GuestRequestKey").Value = "" + Configuration.GuestRequestKey;
                DataSource.allGuests.Add(g);
                saveListToXML(DataSource.allGuests, GuestRequestsPath);
                return g.Copy();
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
                foreach (var item in DataSource.allGuests)
                    if (guestrequest == item)
                    {
                        DataSource.allGuests.Remove(item);
                        DataSource.allGuests.Add(guestrequest);
                        saveListToXML(DataSource.allGuests, GuestRequestsPath);
                        return;
                    }
                throw new OurException();
            }
            catch (OurException)
            {
                throw new OurException("this guest request not found");
            }

        }


        #endregion


        private void updateCofingortion()
        {
            Configuration.hostingUnitKey = int.Parse(configRoot.Element("hostingUnitKey").Value);

            Configuration.GuestRequestKey = int.Parse(configRoot.Element("GuestRequestKey").Value);

            Configuration.OrderKey = int.Parse(configRoot.Element("OrderKey").Value);

            Configuration.commissionFee = int.Parse(configRoot.Element("commissionFee").Value);

        }


       
        public static void saveListToXML<T>(List<T> list, string Path)
        {
            FileStream file = new FileStream(Path, FileMode.Create);
            XmlSerializer x = new XmlSerializer(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }

        private static List<T> loadListFromXML<T>(string path)
        {
            XDocument xdoc = XDocument.Load(path);
            if (xdoc.Descendants().Count()>1)
            {
                List<T> list;
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                FileStream file = new FileStream(path, FileMode.Open);
                list = (List<T>)x.Deserialize(file);
                file.Close();
                return list;
            }
            else return new List<T>();
        }
              
        public List<HostingUnit> GetAllHostingUnits()
        {
            return DataSource.allHostingUnits.Copy();
        }



        public List<GuestRequest> getAllGuests()
        {
            return DataSource.allGuests.Copy();
        }

        public List<Order> getAllOrder()
        {            
            return DataSource.allOrders;
        }
        public void SetDairy(Order ord)
        {
            HostingUnit hoting = hostingUnitExist(ord.HostingUnitKey);
            GuestRequest gu = guestRequestExist(ord.GuestRequestKey);
            for (DateTime i = gu.EntryDate; i < gu.ReleaseDate; i = i.AddDays(1))
            {
                hoting.Diary[i.Month - 1, i.Day - 1] = true;
            }
            updateHostingUnit(hoting);
        }
        /// <summary>
        /// look for hosting unit by name owner
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool didHaveNeme(string name)
        {
            var answer =
                from hu in GetAllHostingUnits()
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
            bool isInt = int.TryParse("" + key, out intKey);
            if (isInt)
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


        public List<BankBranch> GetAllBankBranch()
        {

            return bankBranches;
        }
        private void downloadBanks()
        {
            List<BankBranch> allBancks = new List<BankBranch>();
            const string xmlLocalPath = @"atm.xml";
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
                XElement banksXml = new XElement(xmlLocalPath);
                XDocument banksXmlDoc = XDocument.Load(xmlLocalPath);
                foreach (var item in banksXmlDoc.Elements())
                {
                    foreach (var bank in item.Elements())
                    {
                        BankBranch temp = new BankBranch();
                        foreach (var bankDitails in bank.Elements())
                        {
                            temp.BankName = bankDitails.Name == "שם_בנק" ? bankDitails.Value : temp.BankName;
                            temp.BankNumber = bankDitails.Name == "קוד_בנק" ? int.Parse(bankDitails.Value) : temp.BankNumber;
                            temp.BranchCity = bankDitails.Name == "ישוב" ? bankDitails.Value : temp.BranchCity;
                            temp.BranchAddress = bankDitails.Name == "כתובת_ה-ATM" ? bankDitails.Value : temp.BranchAddress;
                            temp.BranchNumber = bankDitails.Name == "קוד_סניף" ? int.Parse(bankDitails.Value) : temp.BranchNumber;

                        }
                        //if(!allBancks.Exists(x=>x.BankNumber == temp.BranchNumber))
                           allBancks.Add(temp);
                    }

                }
            }
            catch (Exception)
            {
                string xmlServerPath = @"https://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            finally
            {
                wc.Dispose();
            }
            var sortBanks = allBancks.GroupBy(s => s.BranchNumber)
               .Select(s => s.FirstOrDefault()).ToList();
            bankBranches = sortBanks;
            Thread.Sleep(1000);
        }
        private void loadFromXml()
        {
            XDocument xdoc = XDocument.Load(hostingUnitsPath);
            if (xdoc.Descendants().Count() != 0)
                DataSource.allHostingUnits = loadListFromXML<HostingUnit>(hostingUnitsPath);
            xdoc = XDocument.Load(GuestRequestsPath);
            if (xdoc.Descendants().Count() != 0)
                DataSource.allGuests = loadListFromXML<GuestRequest>(GuestRequestsPath);

            foreach (var item in orderRoot.Elements())
            {
                Order ord = new Order();
                ord.HostingUnitKey = int.Parse(item.Element("hostingUnitKey").Value);
                ord.OrderKey = int.Parse(item.Element("orderKey").Value);
                ord.GuestRequestKey = int.Parse(item.Element("guestRequestKey").Value);
                ord.OrderDate = DateTime.Parse(item.Element("orderDate").Value);
                ord.status = int.Parse(item.Element("status").Value);
                ord.CreateDate = DateTime.Parse(item.Element("createDate").Value);
                DataSource.allOrders.Add(ord);
            }

        }
       
    }
}

