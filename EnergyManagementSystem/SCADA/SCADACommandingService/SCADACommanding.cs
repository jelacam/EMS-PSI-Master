//-----------------------------------------------------------------------
// <copyright file="SCADACommanding.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.SCADACommandingService
{
    using Common;
    using CommonMeasurement;
    using ServiceContracts;
    using NetworkModelService.DataModel.Meas;
    using SmoothModbus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Net.Sockets;
    using System.Windows;
    using System.Diagnostics;
    using System.Threading;
    using System.Runtime.InteropServices;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// SCADACommanding class for accept data from CE and put data to simulator
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SCADACommanding : IScadaCMDContract, ITransactionContract
    {
        /// <summary>
        /// Instance of ModbusClient
        /// </summary>
        private ModbusClient modbusClient;

        /// <summary>
        /// List for storing CMDAnalogLocation values
        /// </summary>
        private static List<AnalogLocation> listOfAnalog;

        private static List<AnalogLocation> listOfAnalogCopy;

        private UpdateResult updateResult;

        private ModelResourcesDesc modelResourcesDesc;

        private string message = string.Empty;
        private readonly int START_ADDRESS_GENERATOR = 50;

        /// <summary>
        /// TransactionCallback
        /// </summary>
        private ITransactionCallback transactionCallback;

        /// <summary>
        /// instance of ConvertorHelper class
        /// </summary>
        private ConvertorHelper convertorHelper;

        /// <summary>
        /// Constructor SCADACommanding class
        /// </summary>
        public SCADACommanding()
        {
            ConnectToSimulator();

            listOfAnalog = new List<AnalogLocation>();
            listOfAnalogCopy = new List<AnalogLocation>();

            this.convertorHelper = new ConvertorHelper();
            modelResourcesDesc = new ModelResourcesDesc();
            //CreateCMDAnalogLocation();
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
                //Start simulator EasyModbusServerSimulator.exe
                string appPath = Path.GetFullPath("..\\..\\..\\..\\..\\");
                Process.Start(appPath + "EasyModbusServerSimulator.exe");

                Thread.Sleep(2000);
                ConnectToSimulator();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Transaction

        /// <summary>
        /// Commit method
        /// </summary>
        /// <returns></returns>
        public bool Commit(Delta delta)
        {
            try
            {
                listOfAnalog.Clear();
                foreach (AnalogLocation alocation in listOfAnalogCopy)
                {
                    listOfAnalog.Add(alocation.Clone() as AnalogLocation);
                }

                listOfAnalogCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CMD Transaction: Commit phase successfully finished.");
                Console.WriteLine("Number of Analog values: {0}", listOfAnalog.Count);

                ScadaConfiguration sc = new ScadaConfiguration();
                sc.AnalogsList = listOfAnalog;
                XmlSerializer serializer = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer = new StreamWriter("ScadaConfig.xml");
                serializer.Serialize(writer, sc);

                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CMD Transaction: Failed to Commit changes. Message: {0}", e.Message);
                return false;
            }
        }

        /// <summary>
        /// Prepare method
        /// </summary>
        /// <param name="delta"></param>
        public UpdateResult Prepare(ref Delta delta)
        {
            try
            {
                transactionCallback = OperationContext.Current.GetCallbackChannel<ITransactionCallback>();
                updateResult = new UpdateResult();

                listOfAnalogCopy = new List<AnalogLocation>();
                int iConsumer = 0;
                int iSynchMach = 0;

                // napravi kopiju od originala 
                foreach (AnalogLocation alocation in listOfAnalog)
                {
                    listOfAnalogCopy.Add(alocation.Clone() as AnalogLocation);
                    if (alocation.StartAddress < 50)
                    {
                        iConsumer++;
                    }
                    else
                    {
                        iSynchMach++;
                    }
                }



                Analog analog = null;
                //int i = 0; // analog counter for address
                //int i = listOfAnalogCopy.Count;

                foreach (ResourceDescription analogRd in delta.InsertOperations)
                {
                    analog = ResourcesDescriptionConverter.ConvertTo<Analog>(analogRd);

                    //listOfAnalogCopy.Add(new AnalogLocation()
                    //{
                    //    Analog = analog,
                    //    StartAddress = i * 2, // float value 4 bytes
                    //    Length = 2
                    //});

                    //i++;

                    if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(analog.PowerSystemResource) == EMSType.ENERGYCONSUMER)
                    {

                        listOfAnalogCopy.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = iConsumer++ * 2,
                            Length = 2
                        });
                    }
                    else
                    {

                        listOfAnalogCopy.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = START_ADDRESS_GENERATOR + iSynchMach++ * 2,
                            Length = 2
                        });
                    }
                }

                updateResult.Message = "SCADA CMD Transaction Prepare finished.";
                updateResult.Result = ResultType.Succeeded;
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "SCADA CMD Transaction Prepare finished successfully.");
                transactionCallback.Response("OK");
            }
            catch (Exception e)
            {
                updateResult.Message = "SCADA CMD Transaction Prepare finished.";
                updateResult.Result = ResultType.Failed;
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "SCADA CMD Transaction Prepare failed. Message: {0}", e.Message);
                transactionCallback.Response("ERROR");
            }

            return updateResult;
        }

        /// <summary>
        /// Rollback Method
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            try
            {
                listOfAnalogCopy.Clear();
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Transaction rollback successfully finished!");
                return true;
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceError, "Transaction rollback error. Message: {0}", e.Message);
                return false;
            }
        }

        #endregion Transaction

        /// <summary>
        /// Method instantiates the test data
        /// </summary>
        public void CreateCMDAnalogLocation()
        {
            // TODO treba izmeniti kad se napravi transakcija sa NMS-om
            listOfAnalog = new List<AnalogLocation>();
            for (int i = 0; i < 5; i++)
            {
                Analog analog = new Analog(10000 + i);
                analog.MinValue = 0;
                analog.MaxValue = 5;
                analog.PowerSystemResource = 20000 + i;
                listOfAnalog.Add(new AnalogLocation()
                {
                    Analog = analog,
                    StartAddress = i * 2,
                    Length = 2
                });
            }
        }

        /// <summary>
        /// Method implements integrity update logic for scada cr component
        /// </summary>
        /// <returns></returns>
        public bool InitiateIntegrityUpdate()
        {
            List<ModelCode> properties = new List<ModelCode>(10);
            ModelCode modelCode = ModelCode.ANALOG;
            int iteratorId = 0;
            int resourcesLeft = 0;
            int numberOfResources = 2;


            List<ResourceDescription> retList = new List<ResourceDescription>(5);
            try
            {
                properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

                iteratorId = NetworkModelGDAProxy.Instance.GetExtentValues(modelCode, properties);
                resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = NetworkModelGDAProxy.Instance.IteratorNext(numberOfResources, iteratorId);
                    retList.AddRange(rds);
                    resourcesLeft = NetworkModelGDAProxy.Instance.IteratorResourcesLeft(iteratorId);
                }
                NetworkModelGDAProxy.Instance.IteratorClose(iteratorId);
            }
            catch (Exception e)
            {
                message = string.Format("Getting extent values method failed for {0}.\n\t{1}", modelCode, e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return false;
            }


            listOfAnalog.Clear();

            try
            {
                int iConsumer = 0;
                int iSynchMach = 0;
                foreach (ResourceDescription rd in retList)
                {
                    Analog analog = ResourcesDescriptionConverter.ConvertTo<Analog>(rd);

                    if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(analog.PowerSystemResource) == EMSType.ENERGYCONSUMER)
                    {

                        listOfAnalog.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = iConsumer++ * 2,
                            Length = 2
                        });
                    }
                    else
                    {

                        listOfAnalog.Add(new AnalogLocation()
                        {
                            Analog = analog,
                            StartAddress = START_ADDRESS_GENERATOR + iSynchMach++ * 2,
                            Length = 2
                        });
                    }
                }

                ScadaConfiguration sc = new ScadaConfiguration();
                sc.AnalogsList = listOfAnalog;
                XmlSerializer serializer = new XmlSerializer(typeof(ScadaConfiguration));
                StreamWriter writer = new StreamWriter("ScadaConfig.xml");
                serializer.Serialize(writer, sc);

            }
            catch (Exception e)
            {
                message = string.Format("Conversion to Analog object failed.\n\t{0}", e.Message);
                Console.WriteLine(message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                return false;
            }

            message = string.Format("Integrity update: Number of {0} values: {1}", modelCode.ToString(), listOfAnalog.Count.ToString());
            CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
            Console.WriteLine("Integrity update: Number of {0} values: {1}", modelCode.ToString(), listOfAnalog.Count.ToString());
            return true;
        }

        /// <summary>
        /// Method accepts data from CE and put data to simulator
        /// </summary>
        /// <param name="measurements"></param>
        public bool SendDataToSimulator(List<MeasurementUnit> measurements)
        {
            for (int i = 0; i < measurements.Count; i++)
            {
                AnalogLocation analogLoc = listOfAnalog.Where(x => x.Analog.PowerSystemResource == measurements[i].Gid).SingleOrDefault();
                try
                {
                    float rawVal = convertorHelper.ConvertFromEGUToRawValue(measurements[i].CurrentValue, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);
                    //float rawVal1 = convertorHelper.ConvertFromEGUToRawValue(89, analogLoc.Analog.MinValue, analogLoc.Analog.MaxValue);

                    modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, rawVal);


                    if (analogLoc.Analog.Mrid.Equals("Analog_sm_10"))
                    {
                        //modbusClient.WriteSingleRegister((ushort)analogLoc.StartAddress, rawVal1);
                        using (var txtWriter = new StreamWriter("SentData.txt", true))
                        {

                            // txtWriter.WriteLine(" [" + DateTime.Now + "] " + " The value for " + analogLoc.Analog.Mrid + " that was sent: RAW = " + rawVal1 + " EGU = " + 89);
                            txtWriter.WriteLine(" [" + DateTime.Now + "] " + " The value for " + analogLoc.Analog.Mrid + " that was sent: RAW = " + rawVal + " EGU = " + measurements[i].CurrentValue);

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
                    return false;
                }
            }

            return true;
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