using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace BE
{
    [Serializable]
    public class Order : IComparable
    {
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set; }
        public int status;
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int CompareTo(object obj)
        {
            return ((IComparable)OrderKey).CompareTo(obj);
        }
        public override string ToString()
        {
            return string.Format("HostingUnitKey: {0} GuestRequestKey: {1} OrderKey: {2} status: {3}", HostingUnitKey,GuestRequestKey,OrderKey,(Status)status);
        }
    }
}
