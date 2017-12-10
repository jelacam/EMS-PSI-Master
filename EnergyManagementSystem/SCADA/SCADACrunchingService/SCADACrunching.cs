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

			int arrayLength = value[1];
			Console.WriteLine("Byte count: {0}", arrayLength);

			// startIndex = 2 because first two bytes a metadata
			int[] values = ModbusHelper.GetValueFromByteArray<int>(value, arrayLength, 2);
			Console.Write("Array: ");
			foreach(int v in values)
			{
				Console.Write(" " + v);
			}
			Console.WriteLine();

			return true;
        }

        public void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
