using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class HostingUnit : IComparable, IEnumerable
    {
       public List<bool> help=new List<bool>();
        public List<int> help1=new List<int>();
        public bool[,] Diary = new bool[12, 31];
        public int hostingUnitKey { get; set; }
        public int pricePerNight { get; set; }
        public string city;
        public string adress;
        public int rating { get; set; }
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }
        public bool spa { get; set; }
        public bool flatTv { get; set; }
        public bool airCondition { get; set; }
        public int Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzy { get; set; }
        public bool Garden { get; set; }
        public bool ChilldrensAttractions { get; set; }
        public int Meals { get; set; }
        public List<Order> orders = new List<Order>();
        public string[] nameOfVarible = new string[] {"Hosting unit name: ", "hosting unit key: ", "rating: ","Type: ","Adults: ","children: ","Meals: ",
            "spa: ", "flat tv: ", "air condition: ", "Pool: ","Jacuzzy: ","Garden: ","Childrens attractions: " };
        private void addToList()
        {
            help.Add(spa); help.Add(flatTv); help.Add(airCondition); help.Add(Pool); help.Add(Jacuzzy);
            help.Add(Garden); help.Add(ChilldrensAttractions);
            help1.Add(hostingUnitKey); help1.Add(rating); help1.Add(Type); help1.Add(Adults);
            help1.Add(Children); help1.Add(Meals);
        }
        public int CompareTo(object obj)
            
        {
            return ((IComparable)hostingUnitKey).CompareTo(obj);
        }
        public int area { get; set; }
        
        
        
        public string[] feedback=new string[0];//צריך לסדר את המערך
      
        public IEnumerator GetEnumerator()//implementation of IEnumerator
        {
            return Diary.GetEnumerator();
        }
    }
    
}
