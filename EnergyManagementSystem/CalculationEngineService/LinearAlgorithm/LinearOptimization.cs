using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService.LinearAlgorithm
{
    public class LinearOptimization
    {

        private SolverContext context;

        private float totalCostLinear;

        private object lockObj;
        private float minProduction = 0;
        private float maxProduction = 0;

        public float TotalCost
        {
            get
            {
                return totalCostLinear;
            }

            set
            {
                totalCostLinear = value;
            }
        }

        public LinearOptimization(float minProduction, float maxProduction)
        {
            TotalCost = 0;

            this.minProduction = minProduction;
            this.maxProduction = maxProduction;

            lockObj = new object();
            context = SolverContext.GetContext();
        }

        public Dictionary<long, OptimisationModel> Start(Dictionary<long, OptimisationModel> optModelMap, float consumption, float windSpeed)
        {
            lock (lockObj)
            {
                if (optModelMap.Count() > 0)
                {
                    Model model = context.CreateModel();

                    Dictionary<long, Decision> decisions = new Dictionary<long, Decision>();
                    foreach (var om in optModelMap)
                    {
                        Decision d = new Decision(Domain.RealNonnegative, "d" + om.Value.GlobalId.ToString());
                        model.AddDecision(d);
                        decisions.Add(om.Value.GlobalId, d);
                    }


                    if (0 <= consumption && consumption <= maxProduction)
                    {
                        Decision help;
                        string goal = "";
                        string limit = "limit";
                        string production = consumption.ToString() + "<=";

                        foreach (var optModel in optModelMap)
                        {
                            help = decisions[optModel.Value.GlobalId];
                            Term tLimit;

                            if (optModel.Value.Managable == 0)
                            {
                                tLimit = 0 <= help <= 0;
                            }
                            else if (optModel.Value.WindPct < 100)
                            {
                                tLimit = optModel.Value.MinPower <= help <= (optModel.Value.MaxPower - optModel.Value.MinPower) / 100 * optModel.Value.WindPct + optModel.Value.MinPower;
                            }
                            else
                            {
                                tLimit = optModel.Value.MinPower <= help <= optModel.Value.MaxPower;
                            }
                            goal += help.ToString() + "*" + optModel.Value.Price.ToString() + "+";

                            model.AddConstraint(limit + optModel.Value.GlobalId, tLimit);

                            production += help.ToString() + "+";
                        }

                        production = production.Substring(0, production.Length - 1);
                        production += "<=" + maxProduction.ToString();
                        model.AddConstraint("production", production);

                        goal = goal.Substring(0, goal.Length - 1);
                        model.AddGoal("cost", GoalKind.Minimize, goal);

                        Solution solution = context.Solve(new SimplexDirective());
                        Report report = solution.GetReport();
                        Console.Write("{0}", report);

                        TotalCost = float.Parse(model.Goals.FirstOrDefault().ToDouble().ToString());

                        string name = "";
                        foreach (var item in model.Decisions)
                        {
                            name = item.Name.Substring(1);
                            OptimisationModel optModel = null;
                            if (optModelMap.TryGetValue(long.Parse(name), out optModel))
                            {
                                optModel.LinearOptimizedValue = float.Parse(item.ToDouble().ToString());
                            }
                        }
                    }


                    context.ClearModel();
                }
            }

            return optModelMap;
        }
    }
}
