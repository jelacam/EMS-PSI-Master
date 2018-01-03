using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {

        public DashboardViewModel()
        {
            try
            {
                CeSubscribeProxy.Instance.SubscribeWithCallback(CallbackAction);
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Publisher Service! \n {0}", e.Message);
            }
        }

        public void CallbackAction(object measuremntUI)
        {
            var bv = measuremntUI;
        }

        protected override void OnDispose()
        {
            CeSubscribeProxy.Instance.Unsubscribe();
            base.OnDispose();
        }
    }
}
