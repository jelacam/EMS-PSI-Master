using EMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EMS.Services.CalculationEngineService
{
    public static class LoadCharacteristics
    {
        public static SynchronousMachineCurveModels Load()
        {
            string message = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(SynchronousMachineCurveModels));

            //cloud
            StreamReader reader = new StreamReader("../Resources/SynchronousMachinesCurves.xml");

            //regular
            //StreamReader reader = new StreamReader("../../../../Resources/SynchronousMachinesCurves.xml");
            var value = serializer.Deserialize(reader);

            SynchronousMachineCurveModels curveModel = new SynchronousMachineCurveModels();

            try
            {
                curveModel = value as SynchronousMachineCurveModels;
                message = string.Format("Successfull read characteristic for Synchronous Machines ");
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                Console.WriteLine(message);
            }
            catch (Exception e)
            {
                message = string.Format( "Error while trying to read characteristic for Synchronous Machines: {0}", e.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                Console.WriteLine(message);
            }

            return curveModel;
        }

        public static void Write(SynchronousMachineCurveModels CharacteristicCurves)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SynchronousMachineCurveModels));
            StreamWriter writer = new StreamWriter("SynchronousMachinesCurves.xml");
            serializer.Serialize(writer, CharacteristicCurves);
        }

       

    }
}
