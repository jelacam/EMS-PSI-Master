using EMS.ServiceContracts;
using SmoothModbus;
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
			string function = Enum.GetName(typeof(FunctionCode), value[0]);
			Console.WriteLine("Function executed: {0}", function);

			return true;
        }

        public void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
