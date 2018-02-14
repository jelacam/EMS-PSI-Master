﻿using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;
using System.ServiceModel;

namespace UIClient.PubSub
{
    public class AlarmsEventsSubscribeProxy : IAesSubscribeContract, IDisposable
    {
        private static IAesSubscribeContract proxy;
        private static DuplexChannelFactory<IAesSubscribeContract> factory;
        private static InstanceContext context;

        public IAesSubscribeContract Proxy
        {
            get
            {
                return proxy;
            }
            set
            {
                proxy = value;
            }
        }

        public AlarmsEventsSubscribeProxy(Action<object> callbackAction)
        {
            if (Proxy == null)
            {
                context = new InstanceContext(new AePubSubCallbackService() { CallbackAction = callbackAction });
                factory = new DuplexChannelFactory<IAesSubscribeContract>(context, "AlarmsEventsPubSub");
                Proxy = factory.CreateChannel();
            }
        }


        //public static IAesPubSubContract Instance
        //{
        //    get
        //    {
        //        if (proxy == null)
        //        {
        //            context = new InstanceContext(new AePubSubCallbackService());
        //            factory = new DuplexChannelFactory<IAesPubSubContract>(context, "AlarmsEventsPubSub");
        //            proxy = factory.CreateChannel();
        //        }
        //        return proxy;
        //    }

        //    set
        //    {
        //        if (proxy == null)
        //        {
        //            proxy = value;
        //        }
        //    }
        //}
        

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
        }

        public void Subscribe()
        {
            proxy.Subscribe();
        }

        public void Unsubscribe()
        {
            proxy.Unsubscribe();
        }
    }
}
