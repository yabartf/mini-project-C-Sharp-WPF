using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters;
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
        //public int[] matrix_availability = new int[10] {1,2,3,4,5,6,7,8,9,10 };//to XML serializer
        [XmlIgnore]
        public bool[,] Diary = new bool[12, 31];
        [XmlArray("Diary")]
        public bool[] matrix_availability
        {
            get
            {
                return Diary.Flatten();
                //if (Diary == null)
                //    return null;
                //string result = "";
                //if (Diary != null)
                //{
                //    int sizeA = Diary.GetLength(0);
                //    int sizeB = Diary.GetLength(1);
                //    result += "" + sizeA + "," + sizeB;
                //    for (int i = 0; i < sizeA; i++)
                //        for (int j = 0; j < sizeB; j++)
                //            result += "," + Diary[i, j];
                // }
                //return result;
            }
            set
            {
                Diary = value.Expand(12);
                //if (value != null && value.Length > 0)
                //{
                //    string[] values = value.Split(',');
                //    int sizeA = int.Parse(values[0]);
                //    int sizeB = int.Parse(values[1]);
                //    Diary = new bool[sizeA, sizeB];
                //    int index = 2;
                //    for (int i = 0; i < sizeA; i++)
                //        for (int j = 0; j < sizeB; j++)
                //            Diary[i, j] = bool.Parse(values[index++]);
            }
        }


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
        public static T[] Flatten<T>(this T[,] arr)
        {
            int rows = arr.GetLength(0);
            int columns = arr.GetLength(1);
            T[] arrFlattened = new T[rows * columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var test = arr[i, j];
                    arrFlattened[i * rows + j] = arr[i, j];
                }
            }
            return arrFlattened;
        }
        public static T[,] Expand<T>(this T[] arr, int rows)
        {
            int length = arr.GetLength(0);
            int columns = length / rows;
            T[,] arrExpanded = new T[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    arrExpanded[i, j] = arr[i * rows + j];
                }
            }
            return arrExpanded;
        }
    }
}

    