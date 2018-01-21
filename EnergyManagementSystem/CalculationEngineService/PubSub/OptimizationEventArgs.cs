using EMS.ServiceContracts;
using System;
using System.Collections.Generic;

namespace EMS.Services.CalculationEngineService.PubSub
{
    public class OptimizationEventArgs : EventArgs
    {
        private List<MeasurementUI> optimizationResult;
        private string message;

        public List<MeasurementUI> OptimizationResult
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