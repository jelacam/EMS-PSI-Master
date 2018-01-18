//-----------------------------------------------------------------------
// <copyright file="ResourcesDescriptionConverter.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.CommonMeasurement
{
    using System;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Meas;
    using EMS.Services.NetworkModelService.DataModel.Production;
    using EMS.Services.NetworkModelService.DataModel.Wires;
    using Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// Class for converting resource description
    /// </summary>
    public static class ResourcesDescriptionConverter
    {
        public static T ConvertTo<T>(ResourceDescription rd) where T:IdentifiedObject
        {
            IdentifiedObject io = CreateEntity(rd.Id);

            foreach (Property property in rd.Properties)
            {
                if (property.Id == ModelCode.IDENTIFIEDOBJECT_GID)
                {
                    continue;
                }

                if (property.Type == PropertyType.ReferenceVector)
                {
                    continue;
                }

                io.SetProperty(property);
            }
            return (T)io;
        }

        public static IdentifiedObject CreateEntity(long globalId)
        {
            short type = ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            IdentifiedObject io = null;
            switch ((EMSType)type)
            {
                case EMSType.ANALOG:
                    io = new Analog(globalId);
                    break;

                case EMSType.ENERGYCONSUMER:
                    io = new EnergyConsumer(globalId);
                    break;

                case EMSType.SYNCHRONOUSMACHINE:
                    io = new SynchronousMachine(globalId);
                    break;

                case EMSType.EMSFUEL:
                    io = new EMSFuel(globalId);
                    break;

                default:
                    string message = String.Format("Failed to create entity because specified type ({0}) is not supported.", type);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    throw new Exception(message);
            }

            return io;
        }

        
	}
}