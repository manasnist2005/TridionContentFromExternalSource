using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TridionContentFromExternalSource
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreService cs = new CoreService();
            cs.GetCurrentUser();
            //var tt = cs.GetAllRootCategories("tcm:0-2033-1");
            //var tt1 = cs.GetAllComponentandSchema("tcm:0-2033-1");
            //cs.CheckInItemsWithUserComment("tcm:13-499-16");
            //FillComponentWithExternnalContent.getDatafromXml();
            FillComponentWithExternnalContent.FillComponent();
        }
    }
}
