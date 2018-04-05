//-----------------------------------------------------------------------
// <copyright file="SCADACollecting.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACollectingService
{
    using Common;
    using ServiceContracts;
    using ServiceContracts.ServiceFabricProxy;
    using SmoothModbus;
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

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
        private const int SAMPLE_TIME = 3000; //miliseconds

        //TODO ovo ce trebati da se cita iz konfiguracionog fajla
        private ushort numberOfHoldingRegisters = 104; //how much register will read

        /// <summary>
        /// Initializes a new instance of the <see cref="SCADACollecting" /> class
        /// </summary>
        public SCADACollecting()
        {
            ConnectToSimulator();
        }

        private void ConnectToSimulator()
        {
            try
            {
                modbusClient = new ModbusClient("localhost", 502);
                modbusClient.Connect();
            }
            catch (SocketException)
            {
                Thread.Sleep(2000);
                ConnectToSimulator();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StartCollectingData()
        {
            //modbusClient.WriteSingleRegister(0, 6f);
            Thread.Sleep(5000); // wait some time before start
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
                ScadaCRsfProxy scadaCRsfProxy = new ScadaCRsfProxy();
                isSuccess = scadaCRsfProxy.SendValues(values);
                //isSuccess = ScadaCRProxy.Instance.SendValues(values);
            }
            catch (Exception ex)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                Console.WriteLine("[Method = GetDataFromSimulator] Error: " + ex.Message);
            }

            return isSuccess;
        }
    }
}