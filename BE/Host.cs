using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BE
{
    [Serializable]
    public class Host : IComparable, IEnumerable
    {
        public int HostId { get; set; }
        public int FhoneNumber { get; set; }
        public int BankAccountNumber { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public BankBranch BankBranchDetails { get; set; }
        public bool CollectionClearance { get; set; }
        public List<HostingUnit> HostingUnitCollection;
        private string[] nameOfVarible = new string[] { "Private name: ","Family name: ","Mail address: ", "Host id: ","Fhone number: ",
            "Bank account number: ","Collection clearance: ","Bank branch details: ","HostingUnits: " };
        public override string ToString()
        {
            int i = 0;
            string answer = nameOfVarible[i++] + PrivateName + "\n";
            answer += nameOfVarible[i++] + FamilyName + "\n";
            answer += nameOfVarible[i++] + MailAddress + "\n";
            answer += nameOfVarible[i++] + HostId.ToString() + "\n";
            answer += nameOfVarible[i++] + FhoneNumber.ToString() + "\n";
            answer += nameOfVarible[i++] + BankAccountNumber.ToString() + "\n";
            answer += nameOfVarible[i++] + CollectionClearance.ToString() + "\n";
            answer += nameOfVarible[i++] + BankBranchDetails.ToString() + "\n";
            answer += nameOfVarible[i++] +  "\n";

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
        public void SortUnits()
        {
            HostingUnitCollection.Sort();
        }
       
        public int CompareTo(object obj)
        {
            return ((IComparable)HostId).CompareTo(obj);
        }


    }
}