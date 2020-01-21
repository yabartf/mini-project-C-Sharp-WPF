using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
    public enum Area
    {
        All=1,North,South,Center,Jerusalem
    };
    public enum Type
    {
        Zimmer=1, Hotel, Camping, Vila,Subleate
    };
    public enum Necessity
    {
        Interested = 1,Possible, NotInterested
    };
    public enum Status
    {
        Complate=1, Faild, NotAddressed, SentMail 
    }
    public enum Meals
    {
        NoMeals, HulfPension=2, FullPension
    }
    public enum ChildrensAttractions
    {
        Interested=1, Possible, NotInterested
    };
}