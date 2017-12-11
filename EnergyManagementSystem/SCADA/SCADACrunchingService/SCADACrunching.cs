//-----------------------------------------------------------------------
// <copyright file="SCADACrunching.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACrunchingService
{
	using System;
	using System.ServiceModel;
	using EMS.ServiceContracts;
	using SmoothModbus;

	/// <summary>
	/// SCADACrunching component logic
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SCADACrunching : IScadaCRContract
    {
		/// <summary>
		/// SendValues method implementation
		/// </summary>
		/// <param name="value">values to send</param>
		/// <returns>returns true if success</returns>
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

		/// <summary>
		/// Test method
		/// </summary>
        public void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
