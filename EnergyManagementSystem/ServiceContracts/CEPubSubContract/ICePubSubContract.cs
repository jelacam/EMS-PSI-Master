using System;
using System.ServiceModel;

namespace EMS.ServiceContracts
{
    /// <summary>
    /// Publish Subscribe pattern - interface defines methods for subscribe and unsubscribe to a specific topic
    /// in this case it will be optimization result
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICePubSubCallbackContract))]
    public interface ICePubSubContract
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();

        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void SubscribeWithCallback(Action<object> callbackAction);

        [OperationContract(IsOneWay = false, IsTerminating = true)]
        void Unsubscribe();

        /// <summary>
        /// Opeation which the data source program calls to provide the service with new information
        /// </summary>
        /// <param name="result"></param>
        [OperationContract(IsOneWay = false)]
        void PublishOptimizationResults(MeasurementUI result);
    }

    /// <summary>
    /// Communication between service and client is duplex, so this interface represent a callback contract which
    /// the service uses to pass new information to subscribed list of clients.
    /// </summary>
    public interface ICePubSubCallbackContract
    {
        [OperationContract(IsOneWay = false)]
        void OptimizationResults(MeasurementUI result);

        Action<object> CallbackAction { get; set; }
    }
}