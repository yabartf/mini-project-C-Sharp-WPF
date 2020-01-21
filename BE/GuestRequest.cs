using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class GuestRequest : IComparable, IEnumerable
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
        public int  Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Pool { get; set; }
        public int  Jacuzzy { get; set; }
        public int Garden { get; set; }
        public int ChildrensAttractions { get; set; }
        public int Meals { get; set; }
        public GuestRequest() { addToList(); }
        private string[] nameOfVarible = new string[] {"Guest request key: ","Private name: ","Family name: ","Mail address: " ,"Entry date: ","Release date: ","Registration date: ","status: ","spa: ","flat tv: "
        ,"air condition: ","Pool: ","Jacuzzy: ","Garden: ","Childrens attractions: ","Area: ","Type: ","Adults: ","Children: ","Meals: "};
        public List<int> varible=new List<int>();
        private void addToList()
        {
            varible.Add(spa); varible.Add(flatTv); varible.Add(airCondition); varible.Add(Pool); varible.Add(Jacuzzy); varible.Add(Garden);
            varible.Add(ChildrensAttractions);
        }
        public override string ToString()
        {            
            int i = 0;
            string answer=nameOfVarible[i++]+ GuestRequestKey.ToString()+"\n";
            answer += nameOfVarible[i++] + PrivateName + "\n";
            answer += nameOfVarible[i++] + FamilyName + "\n";
            answer += nameOfVarible[i++] + MailAddress + "\n";
            answer += nameOfVarible[i++] + EntryDate.ToString() + "\n";
            answer += nameOfVarible[i++] + ReleaseDate.ToString() + "\n";
            answer += nameOfVarible[i++] + RegistrationDate.ToString() + "\n";
            answer += nameOfVarible[i++] + status.ToString() + "\n";
            foreach (var item in varible)
            {
                answer += nameOfVarible[i++] + item + "\n";
            }
            answer += nameOfVarible[i++] + Area.ToString() + "\n";
            answer += nameOfVarible[i++] + Type.ToString() + "\n";
            answer += nameOfVarible[i++] + Adults.ToString() + "\n";
            answer += nameOfVarible[i++] + Children.ToString() + "\n";
            answer += nameOfVarible[i++] + Meals.ToString() + "\n";
            return answer;
        }
        public int CompareTo(object obj)
        {
            return ((IComparable)GuestRequestKey).CompareTo(obj);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)varible).GetEnumerator();
        }
    }
}