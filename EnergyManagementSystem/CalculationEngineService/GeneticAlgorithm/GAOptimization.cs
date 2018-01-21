using EMS.CommonMeasurement;
using EMS.Services.CalculationEngineService;
using EMS.Services.CalculationEngineService.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService.GeneticAlgorithm
{
    public class GAOptimization
    {
        private readonly int ELITIMS_PERCENTAGE = 5;
        private readonly int CHROMOSOME_NUMBER = 100;

        private readonly float mutationRate = 0.01f;
        private readonly int MIN_VALUE = 0;
        private readonly int MAX_VALUE = 100;

        private float[] setOfValues;
        private float necessaryEnergy;
        private Random random;
        private GeneticAlgorithm<float> ga;
        private Dictionary<long, OptimisationModel> optModelMap;
        private Dictionary<int, long> indexToGid;

        public GAOptimization(float necessaryEnergy, Dictionary<long, OptimisationModel> optModelMap)
        {
            //InitValues();
            indexToGid = new Dictionary<int, long>();

            int i = 0;
            foreach (var valPair in optModelMap)
            {
                indexToGid.Add(i++, valPair.Key);
            }

            this.necessaryEnergy = necessaryEnergy;
            this.optModelMap = optModelMap;
        }

        public void StartAlgorithm()
        {
            random = new Random();
            ga = new GeneticAlgorithm<float>(1000, optModelMap.Count, random, GetRandomGene, fitnessFunction, ELITIMS_PERCENTAGE, mutationRate);
            
        }

        public List<MeasurementUnit> StartAlgorithmWithReturn()
        {
            random = new Random();
            ga = new GeneticAlgorithm<float>(1000, optModelMap.Count, random, GetRandomGene, fitnessFunction, ELITIMS_PERCENTAGE, mutationRate);

            float[] bestGenes = ga.StartAndReturnBest(100);
            List<MeasurementUnit> retList = new List<MeasurementUnit>();

            for (int i = 0; i < bestGenes.Length; i++)
            {
                retList.Add(new MeasurementUnit()
                {
                    CurrentValue = bestGenes[i],
                    Gid = indexToGid[i],
                    OptimizedGeneric = 1
                });
            }
            return retList;
        }


        private void Callback(float[] bestGenes)
        {
            throw new NotImplementedException();
        }

        private float fitnessFunction(DNA<float> dna)
        {
            float penalty = 1000 * (necessaryEnergy - CalculateEnergy(dna));

            if (penalty < 0)
            {
                penalty = 0;
            }
            return 1000 - (CalculateCost(dna) + penalty); // we need the smallest cost
        }

        private float CalculateEnergy(DNA<float> dna)
        {
            float energySum = 0;

            foreach (var gene in dna.Genes)
            {
                energySum += gene;
            }

            return energySum;
        }

        private float GetRandomGene(int index)
        {

            var minPower = optModelMap[indexToGid[index]].MinPower;
            var maxPower = optModelMap[indexToGid[index]].MaxPower;
            float randNumb = (float)GetRandomNumber(minPower, maxPower);

            return randNumb;
        }

        private double GetRandomNumber(float minPower, float maxPower)
        {
            return random.NextDouble() * (maxPower - minPower) + minPower;
        }

        private float CalculateCost(IList<Tuple<float, float>> generators)
        {
            float cost = 0;

            foreach (Tuple<float, float> generator in generators)
            {
                cost += generator.Item1 * generator.Item2; //Item1 COST, Item2 Energy
            }

            return cost;
        }

        private float CalculateCost(DNA<float> dna)
        {
            List<Tuple<float, float>> generators = new List<Tuple<float, float>>();
            foreach (var keyPair in indexToGid)
            {
                float price = optModelMap[keyPair.Value].Price;
                float energy = dna.Genes[keyPair.Key];
                generators.Add(new Tuple<float, float>(price, energy)); //Item1 COST, Item2 Energy
            }

            return CalculateCost(generators);
        }

    }
}
