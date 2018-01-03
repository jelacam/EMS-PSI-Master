using EMS.ServiceContracts;
using System;

namespace EMS.Services.CalculationEngineService.PubSub
{
    public class OptimizationEventArgs : EventArgs
    {
        private MeasurementUI optimizationResult;
        private string message;

        public MeasurementUI OptimizationResult
        {
            get { return optimizationResult; }
            set { optimizationResult = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}