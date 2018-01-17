using EMS.CommonMeasurement;
using EMS.Services.CalculationEngineService;
using GAF;
using GAF.Extensions;
using GAF.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService
{
    public class GAOptimization
    {
        private readonly int ELITIMS_PERCENTAGE = 5;
        private readonly int CHROMOSOME_NUMBER = 100;
        private readonly long MAX_EVALUATIONS = 10;
        private double necessaryEnergy;

        public GAOptimization(double necessaryEnergy)
        {
            this.necessaryEnergy = necessaryEnergy;
        }


        public void StartAlgorithm()
        {
            Population population = new Population();
            Random rnd = new Random();
            for (int i = 0; i < CHROMOSOME_NUMBER; i++)
            {
                var chromosome = new Chromosome();

                double firstGenerator = rnd.NextDouble() * necessaryEnergy;

                chromosome.Genes.Add(new Gene(firstGenerator));
                chromosome.Genes.Add(new Gene(necessaryEnergy - firstGenerator));
                population.Solutions.Add(chromosome);
            }

            //create the elite operator
            var elite = new Elite(ELITIMS_PERCENTAGE);

            //create the crossover operator
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePoint
            };

            //create the mutation operator
            var mutate = new SwapMutate(0.02);

            //create the GA
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //hook up to some useful events
            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            //add the operators
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //run the GA
            //ga.Run(TerminateFun);
        }

        private bool TerminateFun(Population population, int currentGeneration, long currentEvaluation)
        {
            return false;
        }

        private void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            throw new NotImplementedException();
        }

        public double GetNextDoubleInRange(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private double CalculateFitness(Chromosome chromosome)
        {
            double cost = CalculateCost(chromosome);
            return 1 - cost / 1000;
        }

        private double CalculateCost(IList<Tuple<double, double>> generators)
        {
            double cost = 0;

            foreach (Tuple<double, double> generator in generators)
            {
                cost += generator.Item1 * generator.Item2; //Item1 COST, Item2 Energy
            }

            return cost;
        }

        private double CalculateCost(Chromosome chromosome)
        {
            List<Tuple<double, double>> generators = new List<Tuple<double, double>>();
            generators.Add(new Tuple<double, double>(6, chromosome.Genes[0].RealValue)); //Item1 COST, Item2 Energy
            generators.Add(new Tuple<double, double>(5, chromosome.Genes[1].RealValue));

            return CalculateCost(generators);
        }

        private void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var cost = CalculateCost(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Cost: {2}", e.Generation, fittest.Fitness, cost);
        }

    }
}
