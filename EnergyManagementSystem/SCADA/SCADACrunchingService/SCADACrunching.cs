using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.SCADACrunchingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SCADACrunching : IScadaCRContract
    {
        public bool SendValues(byte[] value)
        {
            throw new NotImplementedException();
        }

        public void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
