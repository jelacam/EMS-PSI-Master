using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;

namespace GatewayService.NetworkModel
{
    public class NetworkModelGDA : INetworkModelGDAContract
    {
        public UpdateResult ApplyUpdate(Delta delta)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - ApplyUpdate");
            throw new NotImplementedException();
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - GetExtentValues");
            throw new NotImplementedException();
        }

        public int GetRelatedValues(long source, List<ModelCode> propIds, Association association)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - GetRelatedValues");
            throw new NotImplementedException();
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - GetValues");
            throw new NotImplementedException();
        }

        public bool IteratorClose(int id)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - IteratorClose");
            throw new NotImplementedException();
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - IteratorNext");
            throw new NotImplementedException();
        }

        public int IteratorResourcesLeft(int id)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - IteratorResourcesLeft");
            throw new NotImplementedException();
        }

        public int IteratorResourcesTotal(int id)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - IteratorResourcesTotal");
            throw new NotImplementedException();
        }

        public bool IteratorRewind(int id)
        {
            ServiceEventSource.Current.Message("NetworkModel GDA - IteratorRewind");
            throw new NotImplementedException();
        }
    }
}
