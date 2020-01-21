using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_01_5840_6639
{
    class HostingUnit : IComparable, IEnumerable
    {
        private static int stSerialKey = 10000000;
        public HostingUnit() { HostingUnitKey = ++stSerialKey; }
        private bool[,] Diary = new bool[12, 31];
        private int HostingUnitKey1;
        private Host Owner;
        private string HostingUnitName;

        public int HostingUnitKey//set and get for the private HostingUnitKey1 
        {
            get => HostingUnitKey1;
            private set => HostingUnitKey1 = stSerialKey;
        }
        public int CompareTo(object obj)
        {
            return GetAnnualBusyDays();
        }

        public override string ToString()
        {
            string answer = HostingUnitKey.ToString() + ":\n";
            DateTime d = new DateTime(2020, 1, 1);
            bool flag = false;                   // for print just first and last date.
            for (; d.Year < 2021; d = d.AddDays(1))
            {
                if (!flag && Diary[d.Month - 1, d.Day - 1]) // just when found first day true. 
                {
                    answer += d.ToString("dd/MM/yyyy") + " - ";
                    flag = true;
                }
                if (flag && !Diary[d.Month - 1, d.Day - 1])    // just when flage=true and when begin be false. 
                {
                    answer += d.AddDays(-1).ToString("dd/MM/yyyy") + "\n";
                    flag = false;
                }
            }

            return answer;
        }
        public bool ApproveRequest(GuestRequest guestReq)
        {
            // check if all is proper.
            for (DateTime tempDate = guestReq.EntryDate; tempDate < guestReq.ReleaseDate; tempDate = tempDate.AddDays(1))
            {
                if (Diary[tempDate.Month - 1, tempDate.Day - 1])
                    return false; // if once wrong instant return false.
            }
            //  update al the dates.
            for (DateTime tempDate = guestReq.EntryDate; tempDate < guestReq.ReleaseDate; tempDate = tempDate.AddDays(1))
            {
                Diary[tempDate.Month - 1, tempDate.Day - 1] = true;
            }
            guestReq.IsApproved = true;
            return true;
        }
        public int GetAnnualBusyDays()
        {
            int counter = 0;
            DateTime d = new DateTime(2020, 1, 1);
            for (; d.Year < 2021; d = d.AddDays(1))
            {
                if (Diary[d.Month - 1, d.Day - 1])//every dat that is accupied counter ++
                {
                    counter++;
                }

            }
            return counter;
        }
        public float GetAnnualBusyPercentage()
        {
            int x = GetAnnualBusyDays();
            return (float)x / 365;
        }

        public IEnumerator GetEnumerator()//implementation of IEnumerator
        {
            return Diary.GetEnumerator();
        }
    }
}
