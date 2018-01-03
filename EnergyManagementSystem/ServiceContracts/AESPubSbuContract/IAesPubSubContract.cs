using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace EMS.ServiceContracts
{
    /// <summary>
    /// Publish Subscribe pattern - interface defines methods for subscribe and unsubscribe to a specific topic
    /// in this case it will be alarms events
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAesPubSubCallbackContract))]
    public interface IAesPubSubContract
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Unsubscribe();

        /// <summary>
        /// Opeation which the data source program calls to provide the service with new information
        /// </summary>
        /// <param name="alarm"></param>
        //[OperationContract(IsOneWay = false)]
        //void PublishAlarmsEvents(string alarm);

    }

    /// <summary>
    /// Communication between service and client is duplex, so this interface represent a callback contract which
    /// the service uses to pass new information to subscribed list of clients.
    /// </summary>
    public interface IAesPubSubCallbackContract
    {
        [OperationContract(IsOneWay = false)]
        void AlarmsEvents(string alarm);
    }
}
