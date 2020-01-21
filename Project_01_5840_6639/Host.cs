using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Project_01_5840_6639
{
    class Host : IComparable, IEnumerable
    {
        public int HostKey, FhoneNumber;
        private string PrivateName, FamilyName, MailAddress;
        private BankAccount bank;
        private bool CollectionClearance;
        public Host() { HostingUnitCollection1 = new List<HostingUnit>(); }
        private List<HostingUnit> HostingUnitCollection1;
        public List<HostingUnit> HostingUnitCollection
        {
            get => HostingUnitCollection1;

            private set => HostingUnitCollection1 = value;
        }
        public Host(int key, int sumHosting)
        {
            HostKey = key;
            HostingUnitCollection1 = new List<HostingUnit>();
            // update how much hostingunit have.
            for (int i = 0; i < sumHosting; i++)
            {
                // HostingUnit temp = new HostingUnit();

                HostingUnitCollection1.Add(new HostingUnit());
            }
        }
        public override string ToString()
        {
            String answer = "";
            foreach (var item in HostingUnitCollection)
            {
                answer += item.ToString() + "\n";
            }

            return answer;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)HostingUnitCollection).GetEnumerator();
        }
        private long SubmitRequest(GuestRequest guestReq)
        {
            int answer = -1;
            foreach (var item in HostingUnitCollection)
            {
                // if  currently hostingunit empty in this day back the key of her and finish.
                if (item.ApproveRequest(guestReq))
                {
                    answer = item.HostingUnitKey;
                    return answer;
                }
            }
            return answer;
        }
        public Int64 GetHostAnnualBusyDays()
        {
            Int64 counterAll = 0;
            foreach (var item in HostingUnitCollection)
            {
                counterAll += item.GetAnnualBusyDays();
            }
            return counterAll;
        }
        public void SortUnits()
        {
            HostingUnitCollection.Sort();
        }
        public bool AssignRequests(params GuestRequest[] requests)
        {
            for (int i = 0; i < requests.Length; i++)
            {
                if (SubmitRequest(requests[i]) == (-1))
                    return false;
            }
            return true;
        }

        public int CompareTo(object obj)//implementation of Icomprble
        {
            return GetHostAnnualBusyDays().CompareTo(obj);
        }

        public HostingUnit this[int key]//indexer for HostingUnit 
        {
            get
            {
                int counter = 0;
                foreach (var item in HostingUnitCollection)
                {
                    if (counter == key)
                        return item;
                    counter++;
                }
                return null;
            }
        }
    }
}
