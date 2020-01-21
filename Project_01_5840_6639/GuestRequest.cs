using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_01_5840_6639
{
    class GuestRequest : IComparable, IEnumerable
    {
        public DateTime EntryDate, ReleaseDate, RegistrationDate;
        public bool IsApproved;
        string PrivateName, FamilyName, MailAddress, RegistrationDate;
        int Area, SubArea, GuestRequestKey, Type, Adults, Children, Pool, Jacuzz, Garden, ChildrensAttractions;


        public int CompareTo(object obj)
        {
            return EntryDate.CompareTo(obj);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string rDay, eDay, IsA;
            eDay = EntryDate.ToString();
            rDay = ReleaseDate.ToString();
            IsA = IsApproved.ToString();
            return ("EntryDate: " + eDay + " ReleaseDate: " + rDay + " IsApproved: " + IsA);
        }
    }
}
