using System;
using System.Collections.Generic;
using System.Linq;
namespace BE
{
    [Serializable]
    public class BankBranch
    {
        public int BankNumber { get; set; }
        public int BranchNumber { get; set; }
        public string BankName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        private string[] nameOfVarible = new string[] { "Bank name: ", "Branch city: ", "Branch address: ", "Bank number: ", "Branch number: ", };
        public override string ToString()
        {
            int i = 0;
            string answer = nameOfVarible[i++] + BankName + "\n";
            answer += nameOfVarible[i++] + BranchCity + "\n";
            answer += nameOfVarible[i++] + BranchAddress + "\n";
            answer += nameOfVarible[i++] + BankNumber.ToString() + "\n";
            answer += nameOfVarible[i++] + BranchNumber.ToString() + "\n";
            return answer;
        }

    }


}
