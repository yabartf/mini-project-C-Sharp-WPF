using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;
/// <summary>
/// 312555840 yair bartfeld
/// 205746639 dvir sivan
/// we finished in 31/12/2019 so did not have we time.
/// all is run. not perfect but we do all that we can.
/// for check this stage in program you need to check all details of all varibale
/// that we create.
/// thank you.
/// </summary>
namespace PL
{
    class Program
    {
        static BL_imp bl = BL_imp.getBl();
        /// <summary>
        /// general screen
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bl.restartLists();
            int ch;
            do
            {
                Console.WriteLine("1: insert guest request");
                Console.WriteLine("2: hosting unit window.");
                Console.WriteLine("3: owner window");
                Console.WriteLine("4: exit");
                ch = int.Parse(Console.ReadLine());
                switch (ch)
                {
                    case 1:
                        try
                        {
                            addGustRequst();
                        }
                        catch (OurException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine(
                            "if you want enter to personal area press '1' if you want to add hosting unit press '2'.");
                        ch = int.Parse(Console.ReadLine());
                        if (ch == 1)
                            personalArea();
                        else if (ch == 2)
                            addHostingUnit();
                        else Console.WriteLine("ERORR!");
                        break;
                    case 3:
                        OwnerWindow();
                        break;
                    default:
                        break;
                }
            } while (ch != 4);
        }
        static void addGustRequst()
        {
            GuestRequest newGuestRequest = new GuestRequest();
            Console.WriteLine("enter entry date,relese date");
            newGuestRequest.EntryDate = DateTime.Parse(Console.ReadLine());
            newGuestRequest.ReleaseDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("enter the amount of adults and chilldren");
            newGuestRequest.Adults= int.Parse(Console.ReadLine());
            newGuestRequest.Children = int.Parse(Console.ReadLine());
            Console.WriteLine("press in wich area do want to vecation? \n" +
                "1: all \n" +
                "2: north \n" +
                "3: south \n" +
                "4: center \n" +
                "5: jerusalem");

            newGuestRequest.Area = int.Parse(Console.ReadLine());
            Console.WriteLine("enter your private name and last name and your Email address.");
            newGuestRequest.PrivateName =Console.ReadLine();
            newGuestRequest.FamilyName = Console.ReadLine();
            newGuestRequest.MailAddress = Console.ReadLine();
            Console.WriteLine("press what type of hosting unit would you like\n" +
                "1: zimmer\n" +
                "2: hotel\n" +
                "3: camping\n" +
                "4: vila\n" +
                "5: sublate");
            newGuestRequest.Type = int.Parse(Console.ReadLine());

            Console.WriteLine("if its neccerry '1', if you are intrsted but its not neccerry press '2', if you are not intrsted press '3' for all bellow:");
            Console.WriteLine("pool, jacuzzy, flat tv, garden children attractions, spa, air condition");
            newGuestRequest.Pool = int.Parse(Console.ReadLine());
            newGuestRequest.Jacuzzy = int.Parse(Console.ReadLine());
            newGuestRequest.flatTv = int.Parse(Console.ReadLine());
            newGuestRequest.Garden = int.Parse(Console.ReadLine());
            newGuestRequest.ChildrensAttractions = int.Parse(Console.ReadLine());
            newGuestRequest.spa = int.Parse(Console.ReadLine());
            newGuestRequest.airCondition = int.Parse(Console.ReadLine());
            Console.WriteLine("how many meals do you want 0,2, or 3");
            newGuestRequest.Meals = int.Parse(Console.ReadLine());
            bl.addGuestRequest(newGuestRequest);

        }
        /// <summary>
        /// personalArea
        /// </summary>
        static void personalArea()
        {
            Console.WriteLine("1: update hosting unit.\n" +
                "2: order\n" +
                "3: delete hosting unit\n" +
                "4: exit");
            int ch = int.Parse(Console.ReadLine());
            while (ch != 4)
            {
                switch (ch)
                {
                    case 1:
                        updateHostingUnit();
                        break;
                    case 2:
                        Order();
                        break;
                    case 3:
                        DeleteHostingUnit();
                        break;
                    default:
                        break;
                }
            }
        }
        static void addHostingUnit()
        {
            
            HostingUnit newHostingUnit = new HostingUnit();
            newHostingUnit = hostigUnitDetails(newHostingUnit);
            bl.addHostingUnit(newHostingUnit);
        }
        static Host addOwner()
        {

            Host host = new Host();
            Console.WriteLine("enter your id");
            host.HostId = int.Parse(Console.ReadLine());
            // DAL
            try
            {
                foreach (var item in bl.GetAllHostingUnits())
                {
                    if (item.Owner == host)
                        return item.Owner;
                }
            }
            catch (OurException ex)
            {
            }
            Console.WriteLine("enter your first name");
            host.PrivateName = Console.ReadLine();
            Console.WriteLine("enter your last name");
            host.FamilyName = Console.ReadLine();
            Console.WriteLine("enter your fone number");
            host.FhoneNumber = int.Parse(Console.ReadLine());
            Console.WriteLine("enter your mail address");
            host.MailAddress = Console.ReadLine();
            Console.WriteLine("enter bank number");
            host.BankBranchDetails = bankByBankNumber(int.Parse(Console.ReadLine()));
            Console.WriteLine("enter bank account number");
            host.BankAccountNumber = int.Parse(Console.ReadLine());
            host.CollectionClearance = true;
            return host;
        }
        static BankBranch bankByBankNumber(int number)
        {
           
            foreach (var item in bl.GetAllBankBranch())
            {
                if (item.BankNumber == number)
                    return item;
            }
            throw new OurException();
        }
        static void updateHostingUnit()
        {
            
            HostingUnit upHostingUnit = new HostingUnit();
            upHostingUnit = hostigUnitDetails(upHostingUnit);
            bl.updateHostingUnit(upHostingUnit); 
        }

        /// <summary>
        /// 
        /// </summary>
        static void Order()
        {
            Order order = new Order();
            Console.WriteLine("enter yor id:");
            int id = int.Parse(Console.ReadLine());
            List<HostingUnit> yourHostingUnits = bl.ownersHostingUnits(id);
            Console.WriteLine("enter the order key of the order that you want update.");
            foreach (var item in yourHostingUnits)
                foreach (var orderItem in item.orders)
                {
                    Console.WriteLine(orderItem);
                }
            order.OrderKey = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the order key of the hosting unit that have that order.");
            order.HostingUnitKey = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the order key of the guest request that you want to updat his order.");
            order.GuestRequestKey = int.Parse(Console.ReadLine());
            Console.WriteLine("enter wihch status change to:\n" +
                              "1: complate\n" +
                              "2: faild\n" +
                              "3: NotAddressed\n" +
                              "4: SentMail");
            order.status = int.Parse(Console.ReadLine());
            bl.updateOrder(order);
        }
    
        static void DeleteHostingUnit()
        {
            Console.WriteLine("enter the hosting unit key.");
            int inputKey = int.Parse(Console.ReadLine());
            if (!bl.deleteHostingUnit(inputKey))
                throw new OurException();
            else
                Console.WriteLine("deleted");
        }

        static void OwnerWindow()
        {
            int num;


            do
            {
                Console.WriteLine("enter your choice:\n" +
                              "1: list of all guest requests\n" +
                              "2: list of all hosting unit\n" +
                              "3: list of all orders\n" +
                              "4: exit");/// need to add!///
                num = int.Parse(Console.ReadLine());
                switch (num)
                {
                    case 1:
                        try
                        {
                            foreach (var item in bl.GetAllGuests())
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch (OurException ex)
                        {

                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case 2:
                        try
                        {
                            foreach (var item in bl.GetAllHostingUnits())
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch (OurException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        break;
                    case 3:
                        try
                        {
                            foreach (var item in bl.getAllOrder())
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch (OurException ex)
                        {

                            Console.WriteLine(ex.Message);
                        }

                        break;
                    default:
                        break;
                }
            } while (num != 4);
        }
        static HostingUnit hostigUnitDetails(HostingUnit hostingUnit)
        {
            hostingUnit.Owner = addOwner();
            Console.WriteLine("if you have this item in your hosting unit enter 'true'else enter 'false' for all bellow:");
            Console.WriteLine("pool, jacuzzy, flat tv, garden children attractions, spa, air condition");
            hostingUnit.Jacuzzy = bool.Parse(Console.ReadLine());
            hostingUnit.flatTv = bool.Parse(Console.ReadLine());
            hostingUnit.Garden = bool.Parse(Console.ReadLine());
            hostingUnit.ChilldrensAttractions = bool.Parse(Console.ReadLine());
            hostingUnit.spa = bool.Parse(Console.ReadLine());
            hostingUnit.airCondition = bool.Parse(Console.ReadLine());
            hostingUnit.Pool = bool.Parse(Console.ReadLine());
            Console.WriteLine("press what type of hosting unit\n" +
               "1: zimmer\n" +
               "2: hotel\n" +
               "3: camping\n" +
               "4: vila\n" +
               "5: sublate");
            hostingUnit.Type = int.Parse(Console.ReadLine());
            Console.WriteLine("press in wich area it excist\n" +
               "2: north \n" +
               "3: south \n" +
               "4: center \n" +
               "5: jerusalem");
            hostingUnit.area = int.Parse(Console.ReadLine());
            Console.WriteLine("how many meals do you privied?0, 2 or 3");
            hostingUnit.Meals = int.Parse(Console.ReadLine());
            Console.WriteLine("for how many adults is this hosting unit is sutible?");
            hostingUnit.Adults = int.Parse(Console.ReadLine());
            Console.WriteLine("for how many chilldren is this hosting unit is sutible?");
            hostingUnit.chilldren = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the hosting unit name.");
            hostingUnit.HostingUnitName = Console.ReadLine();
            return hostingUnit;
        }
    }

}
