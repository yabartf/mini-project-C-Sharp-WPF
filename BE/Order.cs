using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Order:IComparable
    {
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey{ get; set; }
        public int OrderKey { get; set; }
        public int status;
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        private string[] nameOfVarible = new string[] { "Hosting unitKey: ", "Guest request key: ", "Order key: ", "status: ", "Create date: ", "Order date: " };
    public override string ToString()
        {
            int i = 0;
            string answer = nameOfVarible[i++] + HostingUnitKey.ToString() + "\n";
            answer+= nameOfVarible[i++] + GuestRequestKey.ToString() + "\n";
            answer+= nameOfVarible[i++] + OrderKey.ToString() + "\n";
            answer+= nameOfVarible[i++] + status.ToString() + "\n";
            answer+= nameOfVarible[i++] + CreateDate.ToString() + "\n";
            answer += nameOfVarible[i++] + OrderDate.ToString() + "\n";
            return answer;
        }
        public int CompareTo(object obj)
        {
            return ((IComparable)OrderKey).CompareTo(obj);
        }
    }
}
