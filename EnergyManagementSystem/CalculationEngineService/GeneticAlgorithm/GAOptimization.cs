using EMS.Common;
using System;
using System.Collections.Generic;

namespace EMS.Services.CalculationEngineService.GeneticAlgorithm
{
    public class GAOptimization
    {
        private readonly int ELITIMS_PERCENTAGE = 5;
        private readonly int NUMBER_OF_ITERATION = 20000;
        private readonly int NUMBER_OF_POPULATION = 100;

        private readonly float mutationRate = 1f;
        private readonly float penaltyRate = 1500;
        private readonly float costRate = 10;

        private float necessaryEnergy;
        private Random random;
        private GeneticAlgorithm<Tuple<long, float>> ga;
        private Dictionary<long, OptimisationModel> optModelMap;
        private Dictionary<int, long> indexToGid;

        public float TotalCost { get; private set; }
        public float EmissionCO2 { get; private set; }
        public float GeneratedPower { get; private set; }

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
            ga = new GeneticAlgorithm<Tuple<long, float>>(NUMBER_OF_POPULATION, optModelMap.Count, random, GetRandomGene, FitnessFunction,
                                             MutateFunction, ELITIMS_PERCENTAGE, mutationRate);

        }

        public Dictionary<long, OptimisationModel> StartAlgorithmWithReturn()
        {
            random = new Random();
            ga = new GeneticAlgorithm<Tuple<long, float>>(NUMBER_OF_POPULATION, optModelMap.Count, random, GetRandomGene, FitnessFunction, MutateFunction, ELITIMS_PERCENTAGE, mutationRate, false);
            ga.Population = PopulateFirstPopulation();
            Tuple<long, float>[] bestGenes = ga.StartAndReturnBest(NUMBER_OF_ITERATION);

            for (int i = 0; i < bestGenes.Length; i++)
            {
                optModelMap[indexToGid[i]].GenericOptimizedValue = bestGenes[i].Item2;
            }

            float emCO2 = CalculationEngine.CalculateCO2(optModelMap);

            EmissionCO2 = emCO2;

            TotalCost = CalculateCost(bestGenes);
            GeneratedPower = CalculateEnergy(bestGenes);

            return optModelMap;
        }

        private List<DNA<Tuple<long, float>>> PopulateFirstPopulation()
        {
            List<DNA<Tuple<long, float>>> firstPopulation = new List<DNA<Tuple<long, float>>>();

            DNA<Tuple<long, float>> previousBest = new DNA<Tuple<long, float>>(optModelMap.Count, random, GetRandomGene, FitnessFunction, MutateFunction, shouldInitGenes: false);
            previousBest.Genes = new Tuple<long, float>[optModelMap.Count];

            for (int i = 0; i < optModelMap.Count; i++)
            {
                long gid = indexToGid[i];
                previousBest.Genes[i] = new Tuple<long, float>(gid, optModelMap[gid].MeasuredValue);
            }
            firstPopulation.Add(previousBest);

            for (int i = 1; i < NUMBER_OF_POPULATION; i++)
            {
                firstPopulation.Add(new DNA<Tuple<long, float>>(optModelMap.Count, random, GetRandomGene, FitnessFunction, MutateFunction, shouldInitGenes: true));
            }

            return firstPopulation;
        }

        private void Callback(Tuple<long, float>[] bestGenes)
        {
            throw new NotImplementedException();
        }

        private float FitnessFunction(DNA<Tuple<long, float>> dna)
        {
            float penaltyOffset = 0;
            float penalty = (penaltyRate + penaltyOffset) * (necessaryEnergy - CalculateEnergy(dna.Genes));

            penalty = Math.Abs(penalty); //we need a value closer to the desired one
            float offset = 1;
            return 1000 - ((costRate + offset) * CalculateCost(dna.Genes) + penalty); // we need the smallest cost
        }

        private float CalculateEnergy(Tuple<long, float>[] genes)
        {
            float energySum = 0;

            foreach (var gene in genes)
            {
                energySum += gene.Item2;
            }

            return energySum;
        }

        private Tuple<long, float> GetRandomGene(int index)
        {

            long gid = indexToGid[index];

            if (optModelMap[gid].Renewable)
            {
                //TODO ispraviti kad se napravi griva na osnovu vetra
                return new Tuple<long, float>(gid, optModelMap[gid].MaxPower);
            }
            else
            {
                var minPower = optModelMap[gid].MinPower;
                var maxPower = optModelMap[gid].MaxPower;
                float randNumb = (float)GetRandomNumber(minPower, maxPower);
                return new Tuple<long, float>(gid, randNumb);
            }
        }

        private Tuple<long, float> MutateFunction(Tuple<long, float> gene, float mutateRate)
        {
            long gid = gene.Item1;
            //if (optModelMap[gid].Renewable) // no mutation for renewable generator
            //{
            //    return gene;
            //}

            double rndNumber = random.NextDouble();
            float mutatedGeneValue = rndNumber < 0.5 ? gene.Item2 + mutateRate : gene.Item2 - mutateRate;


            if (mutatedGeneValue < optModelMap[gid].MinPower)
            {
                mutatedGeneValue = optModelMap[gid].MinPower;
            }
            else if (mutatedGeneValue > optModelMap[gid].MaxPower)
            {
                mutatedGeneValue = optModelMap[gid].MaxPower;
            }
            return new Tuple<long, float>(gid, mutatedGeneValue);
        }

        private double GetRandomNumber(float minPower, float maxPower)
        {
            return random.NextDouble() * (maxPower - minPower) + minPower;
        }

        private float CalculateCost(Tuple<long, float>[] genes)
        {
            float cost = 0;
            foreach (Tuple<long, float> gene in genes)
            {
                float price = optModelMap[gene.Item1].CalculatePrice(gene.Item2);
				optModelMap[gene.Item1].Price = price;
                cost += price;
            }

            return cost;
        }

    }
}
