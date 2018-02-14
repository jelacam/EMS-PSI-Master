using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using EMS.CommonMeasurement;

namespace EMS.ServiceContracts
{
    /// <summary>
    /// Publish Subscribe pattern - interface defines methods for subscribe and unsubscribe to a specific topic
    /// in this case it will be alarms events
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAesSubscribeCallbackContract))]
    public interface IAesSubscribeContract
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Unsubscribe();
    }

    /// <summary>
    /// Communication between service and client is duplex, so this interface represent a callback contract which
    /// the service uses to pass new information to subscribed list of clients.
    /// </summary>
    public interface IAesSubscribeCallbackContract
    {
        [OperationContract(IsOneWay = false)]
        void AlarmsEvents(AlarmHelper alarm);

        [OperationContract(IsOneWay = false)]
        void UpdateAlarmsEvents(AlarmHelper alarm);
    }
}