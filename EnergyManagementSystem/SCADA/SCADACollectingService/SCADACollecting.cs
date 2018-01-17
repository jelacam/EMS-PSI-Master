//-----------------------------------------------------------------------
// <copyright file="SCADACollecting.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACollectingService
{
    using Common;
    using ServiceContracts;
    using SmoothModbus;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// SCADACollecting component logic
    /// </summary>
    public class SCADACollecting : IScadaCLContract
    {
        /// <summary>
        /// Instance of ModbusClient
        /// </summary>
        private ModbusClient modbusClient;

        /// <summary>
        /// Time between 2 samples in miliseconds
        /// </summary>
        private const int SAMPLE_TIME = 2500; //miliseconds

        //TODO ovo ce trebati da se cita iz konfiguracionog fajla
        private ushort numberOfHoldingRegisters = 100; //how much register will read

        /// <summary>
        /// Initializes a new instance of the <see cref="SCADACollecting" /> class
        /// </summary>
        public SCADACollecting()
        {
            modbusClient = new ModbusClient("localhost", 502);
            modbusClient.Connect();

            //modbusClient.WriteSingleRegister(0, 10f);
            //modbusClient.WriteSingleRegister(2, 7f);
            //modbusClient.WriteSingleRegister(4, 7f);
        }

        public void StartCollectingData()
        {
            //modbusClient.WriteSingleRegister(0, 6f);
            Task task = new Task(() =>
            {
                while (true)
                {
                    GetDataFromSimulator();
                    Thread.Sleep(SAMPLE_TIME);
                }
            });
            task.Start();
        }

        /// <summary>
        /// Method for getting data values from simulator
        /// </summary>
        /// <returns> true if values are successfully returned </returns>
        public bool GetDataFromSimulator()
        {
            var values = modbusClient.ReadHoldingRegisters(0, numberOfHoldingRegisters);

            bool isSuccess = false;
            try
            {
                isSuccess = ScadaCRProxy.Instance.SendValues(values);
            }
            catch (System.Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
            }

            return isSuccess;
        }
    }
}