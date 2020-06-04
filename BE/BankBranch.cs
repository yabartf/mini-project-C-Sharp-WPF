using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    [Serializable]
    public class BankBranch
    {
       public int BankNumber { get; set; }
       public int BranchNumber{ get; set; }
       public string BankName { get; set; }
       public string BranchAddress { get; set; }
       public string BranchCity { get; set; }
        
    }


}
