using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService
{
    public class SynchronousMachineCurveModels
    {
        private List<SynchronousMachineCurveModel> curves = new List<SynchronousMachineCurveModel>(20);

        public List<SynchronousMachineCurveModel> Curves
        {
            get { return curves; }
            set { curves = value; }
        }

        public SynchronousMachineCurveModels()
        {

        }
        
    }

    public class SynchronousMachineCurveModel
    {
        private string mrid = string.Empty;
        private double a = 0;
        private double b = 0;
        private double c = 0;

        public SynchronousMachineCurveModel()
        {

        }

        public SynchronousMachineCurveModel(string mrid, double a, double b, double c)
        {
            MRId = mrid;
            A = a;
            B = b;
            C = c;
        }

        public string MRId
        {
            get { return mrid; }
            set { mrid = value; }
        }

        public double A
        {
            get { return a; }
            set { a = value; }
        }

        public double B
        {
            get { return b; }
            set { b = value; }
        }

        public double C
        {
            get { return c; }
            set { c = value; }
        }
    }
}
