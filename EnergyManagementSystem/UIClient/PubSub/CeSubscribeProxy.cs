using System;
using System.ServiceModel;
using EMS.ServiceContracts;
using EMS.Common;
using EMS.CommonMeasurement;

namespace UIClient.PubSub
{
    public class CeSubscribeProxy : ICePubSubContract, IDisposable
    {
        private ICePubSubContract proxy;
        private DuplexChannelFactory<ICePubSubContract> factory;
        private InstanceContext context;

        public ICePubSubContract Proxy
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

        public CeSubscribeProxy(Action<object> callbackAction)
        {
            if (Proxy == null)
            {
                context = new InstanceContext(new CePubSubCallbackService() { CallbackAction = callbackAction });
                factory = new DuplexChannelFactory<ICePubSubContract>(context, "CalculationEnginePubSub");
                Proxy = factory.CreateChannel();
            }
        }

        /*  public static ICePubSubContract Instance
          {
              get
              {
                  if (proxy == null)
                  {
                      context = new InstanceContext(new CePubSubCallbackService());
                      factory = new DuplexChannelFactory<ICePubSubContract>(context, "PubSub");
                      proxy = factory.CreateChannel();
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
          }*/

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
        }

        //public void PublishOptimizationResults(MeasurementUI result)
        //{
        //    CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Client does not have permissions to publis optimization results!");
        //    throw new Exception("Client does not have permissions to publis optimization results!");
        //}

        public void Subscribe()
        {
            Proxy.Subscribe();
        }

        public void Unsubscribe()
        {
            Proxy.Unsubscribe();
        }

        public bool ChooseOptimization(OptimizationType optimizationType)
        {
            return Proxy.ChooseOptimization(optimizationType);
        }
    }
}