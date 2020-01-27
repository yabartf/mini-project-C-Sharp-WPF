using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;







namespace BE
{
    [Serializable]
    public class HostingUnit : IComparable
    {

        [XmlIgnore]
        public bool[,] Diary = new bool[12, 31];

        //[XmlArray("Diary")]
        //public bool[] DiaryToXml
        //{
        //    get { return Diary.Flatten(); }
        //    set { Diary = value.Expand(12); }
        //}

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

        public int CompareTo(object obj)

        {
            return ((IComparable)hostingUnitKey).CompareTo(obj);
        }

        

        public int area { get; set; }

    }
    public static class Tools
    {
        public static bool[] Flatten(this bool[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            bool[] arrFlattened = new bool[rows * columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    var test = arr[i, j];
                    arrFlattened[i * rows + j] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static bool[,] Expand(this bool[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            bool[,] arrExpanded = new bool[rows, columns];
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    arrExpanded[i, j] = arr[i + j * rows];
                }
            }
            return arrExpanded;
        }


    }

}
