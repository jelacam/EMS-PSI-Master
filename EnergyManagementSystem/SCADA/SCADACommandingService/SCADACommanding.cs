using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using EMS.Services.NetworkModelService.DataModel.Meas;
using SmoothModbus;
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

        private ModbusClient modbusClient;

        /// <summary>
        /// list for storing CMDAnalogLocation values
        /// </summary>
        private List<CMDAnalogLocation> listOfAnalog;

        public SCADACommanding()
        {
            this.modbusClient = new ModbusClient("localhost", 502);
            this.modbusClient.Connect();

            CreateCMDAnalogLocation();
        }

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
                    StartAddress = i * 4, // float value 4bytes
                    Length = 4
                });
            }
        }

        public void SendDataToSimulator(List<MeasurementUnit> measurements)
        {
            for(int i = 0; i < measurements.Count; i++)
            {
                CMDAnalogLocation analogLoc = listOfAnalog.Where(x => x.Analog.PowerSystemResource == measurements[i].Gid).SingleOrDefault();
                modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, measurements[i].CurrentValue);
            }
        }

        public void TestWrite()
        {
            for (int i = 0; i < 5; i++)
            {
                modbusClient.WriteSingleRegister((ushort)(i * 4), i * 10 + 10);
            }
        }

        public void Test()
        {
            Console.WriteLine("Test method");
        }
    }
}
