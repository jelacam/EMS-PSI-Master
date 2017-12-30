using System;

namespace EMS.Services.CalculationEngineService.PubSub
{
    public class OptimizationEventArgs : EventArgs
    {
        private float optimizationResult;
        private string message;

        public float OptimizationResult
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