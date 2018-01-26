//-----------------------------------------------------------------------
// <copyright file="NetworkModelGDAProxy.cs" company="EMS-Team">
// Copyright (c) EMS-Team. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EMS.ServiceContracts
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using EMS.Common;
    using System.Threading;

    /// <summary>
    /// Represent proxy class for communication with NetworkModelService
    /// </summary>
    public class NetworkModelGDAProxy : INetworkModelGDAContract, IDisposable
    {
        /// <summary>
        /// INetworkModelGDAContract instace for proxy
        /// </summary>
        private static INetworkModelGDAContract proxy;

        /// <summary>
        /// ChannelFactory instance for INetworkModelGDAContract
        /// </summary>
        private static ChannelFactory<INetworkModelGDAContract> factory;

        private static object lockObj = new object();

        /// <summary>
        /// Gets singleton instance for INetworkModelGDAContract
        /// Sets singleton instance for INetworkModelGDAContract
        /// </summary>
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
                {
                    if (proxy != value)
                    {
                        proxy = value;
                    }
                }
            }
        }

        /// <summary>
        /// Dispose method
        /// </summary>
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
            catch (Exception e)
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
            lock (lockObj)
            {
                return proxy.GetExtentValues(entityType, propIds);
            }
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
