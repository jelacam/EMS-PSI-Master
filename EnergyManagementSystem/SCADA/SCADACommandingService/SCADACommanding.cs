using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.SCADACommandingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SCADACommanding : IScadaCMDContract
    {
        public void Test()
        {
            Console.WriteLine("Test method");


        }
    }
}
