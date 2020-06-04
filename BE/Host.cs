using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;

namespace BE
{
    [Serializable]
    public class Host : IComparable
    {
        public int HostId { get; set; }
        public int FhoneNumber { get; set; }
        public int BankAccountNumber { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public BankBranch BankBranchDetails { get; set; }
        public bool CollectionClearance { get; set; }
      
        //public void SortUnits()
        //{
        //    HostingUnitCollection.Sort();
        //}

        public int CompareTo(object obj)
        {
            return ((IComparable)HostId).CompareTo(obj);
        }


    }
}