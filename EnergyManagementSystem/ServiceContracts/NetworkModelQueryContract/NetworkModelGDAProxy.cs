using System.Collections.Generic;
using EMS.Common;
using System.ServiceModel;
using System;
using System.Configuration;

namespace EMS.ServiceContracts
{
	public class NetworkModelGDAProxy : INetworkModelGDAContract, IDisposable
	{
        //public NetworkModelGDAProxy(string endpointName)
        //	: base(endpointName)
        //{
        //}

        private static INetworkModelGDAContract proxy;
        private static ChannelFactory<INetworkModelGDAContract> factory;

        //private static EndpointAddress = NetworkModelGDAAddress;
       
        public static INetworkModelGDAContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    factory = new ChannelFactory<INetworkModelGDAContract>("*");
                    proxy = factory.CreateChannel();

                    IContextChannel cc = proxy as IContextChannel;
                }

                return proxy;
            }
            set
            {
                if (proxy == null)
                {
                    proxy = value;
                }
            }
        }


        public void Dispose()
        {
            try
            {
                if (factory != null)
                {
                    factory = null;
                }
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("Communication exception: {0}", ce.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("GDA proxy exception: {0}", e.Message);
            }
        
        }

		public UpdateResult ApplyUpdate(Delta delta)
		{
			return proxy.ApplyUpdate(delta);
		}

		public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
		{
			return proxy.GetValues(resourceId, propIds);
		}

		public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
		{
			return proxy.GetExtentValues(entityType, propIds); 
		}	

		public int GetRelatedValues(long source, List<ModelCode> propIds, Association association)
		{
			return proxy.GetRelatedValues(source, propIds, association);
		}

		
		public bool IteratorClose(int id)
		{
			return proxy.IteratorClose(id);
		}

		public List<ResourceDescription> IteratorNext(int n, int id)
		{
			return proxy.IteratorNext(n, id);
		}

		public int IteratorResourcesLeft(int id)
		{
			return proxy.IteratorResourcesLeft(id);
		}

		public int IteratorResourcesTotal(int id)
		{
			return proxy.IteratorResourcesTotal(id);
		}

		public bool IteratorRewind(int id)
		{
			return proxy.IteratorRewind(id);
		}
	}
}
