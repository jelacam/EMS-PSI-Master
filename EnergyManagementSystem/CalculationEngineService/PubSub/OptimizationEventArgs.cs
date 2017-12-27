using System;

namespace EMS.Services.CalculationEngineService.PubSub
{
    public class OptimizationEventArgs : EventArgs
    {
        private int optimizationResult;
        private string message;

        public int OptimizationResult
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