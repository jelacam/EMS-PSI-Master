//-----------------------------------------------------------------------
// <copyright file="SCADACommanding.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.Services.SCADACommandingService
{
    using EMS.Common;
    using EMS.CommonMeasurement;
    using EMS.ServiceContracts;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using SmoothModbus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// SCADACommanding class for accept data from CE and put data to simulator
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SCADACommanding : IScadaCMDContract
    {
        /// <summary>
        /// Instance of ModbusClient
        /// </summary>
        private ModbusClient modbusClient;

        /// <summary>
        /// List for storing CMDAnalogLocation values
        /// </summary>
        private List<CMDAnalogLocation> listOfAnalog;

        /// <summary>
        /// Constructor SCADACommanding class
        /// </summary>
        public SCADACommanding()
        {
            this.modbusClient = new ModbusClient("localhost", 502);
            this.modbusClient.Connect();

            CreateCMDAnalogLocation();
        }

        /// <summary>
        /// Method instantiates the test data
        /// </summary>
        public void CreateCMDAnalogLocation()
        {
            // TODO treba izmeniti kad se napravi transakcija sa NMS-om
            this.listOfAnalog = new List<CMDAnalogLocation>();
            for (int i = 0; i < 5; i++)
            {
                Analog analog = new Analog(10000 + i);
                analog.MinValue = 0;
                analog.MaxValue = 5;
                analog.PowerSystemResource = 20000 + i;
                this.listOfAnalog.Add(new CMDAnalogLocation()
                {
                    Analog = analog,
                    StartAddress = i * 2,
                    Length = 2
                });
            }
        }

        /// <summary>
        /// Method accepts data from CE and put data to simulator
        /// </summary>
        /// <param name="measurements"></param>
        public void SendDataToSimulator(List<MeasurementUnit> measurements)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                CMDAnalogLocation analogLoc = listOfAnalog.Where(x => x.Analog.PowerSystemResource == measurements[i].Gid).SingleOrDefault();
                try
                {
                    modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, measurements[i].CurrentValue);
                }
                catch (System.Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Method for initial write in simulator
        /// </summary>
        public void TestWrite()
        {
            for (int i = 0; i < listOfAnalog.Count; i++)
            {
                try
                {
                    modbusClient.WriteSingleRegister((ushort)listOfAnalog[i].StartAddress, i * 10 + 10);
                }
                catch (System.Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Test connection method
        /// </summary>
        public void Test()
        {
            Console.WriteLine("Test method");
        }
    }
}
