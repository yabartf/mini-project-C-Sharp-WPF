using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class GuestRequest : IComparable
    {
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int status { get; set; }
        public int spa { get; set; }
        public int flatTv { get; set; }
        public int airCondition { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public int Area { get; set; }
        public int GuestRequestKey { get; set; }
        public int Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Pool { get; set; }
        public int Jacuzzy { get; set; }
        public int Garden { get; set; }
        public int ChildrensAttractions { get; set; }
        public int Meals { get; set; }
        public int CompareTo(object obj)
        {
            return ((IComparable)GuestRequestKey).CompareTo(obj);
        }

        
    }
}