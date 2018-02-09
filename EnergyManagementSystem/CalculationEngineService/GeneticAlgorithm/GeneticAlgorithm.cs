using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Services.CalculationEngineService.GeneticAlgorithm
{
    public class GeneticAlgorithm<T>
    {
        public List<DNA<T>> Population { get; set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public T[] BestGenes { get; private set; }

        public int Elitism;
        public float MutationRate;

        private List<DNA<T>> newPopulation;
        private Random random;
        private float fitnessSum;
        private int dnaSize;
        private Func<int, T> getRandomGene;
        private Func<DNA<T>, float> fitnessFunction;
        private Func<T, float, T> mutateFunction;

        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<int, T> getRandomGene, Func<DNA<T>, float> fitnessFunction,
            Func<T, float, T> mutateFunction, int elitism, float mutationRate = 0.01f, bool shoudlInitPopulation = true)
        {
            Generation = 1;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<DNA<T>>(populationSize);
            newPopulation = new List<DNA<T>>(populationSize);
            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            this.mutateFunction = mutateFunction;

            BestGenes = new T[dnaSize];

            if (shoudlInitPopulation)
            {
                for (int i = 0; i < populationSize; i++)
                {
                    Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, mutateFunction, shouldInitGenes: true));
                }
            }
        }

        internal void Start(int numOfIterations, Action<T[]> callback)
        {
            T[] bestGenes = StartAndReturnBest(numOfIterations);
            callback.Invoke(bestGenes);
        }

        internal T[] StartAndReturnBest(int numOfIterations)
        {
            for (int i = 0; i < numOfIterations; i++)
            {
                NewGeneration(20);
            }

            return BestGenes;
        }


        public void NewGeneration(int numNewDNA = 0)
        {
            int finalCount = Population.Count + numNewDNA;

            if (finalCount <= 0)
            {
                return;
            }

            if (Population.Count > 0)
            {
                CalculateFitness();
                Population.Sort(CompareDNA);
            }
            newPopulation.Clear();

            for (int i = 0; i < Population.Count; i++)
            {
                if (i < Elitism && i < Population.Count)
                {
                    newPopulation.Add(Population[i]);
                }
                else if (i < Population.Count - numNewDNA)
                {
                    DNA<T> parent1 = ChooseParent();
                    DNA<T> parent2 = ChooseParent();
                    DNA<T> child = null;

                    if (parent1 == null || parent2 == null)
                    {
                        child = new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, mutateFunction, shouldInitGenes: true);
                    }
                    else
                    {
                        child = parent1.Crossover(parent2);
                    }

                    child.Mutate(MutationRate);

                    newPopulation.Add(child);
                }
                else
                {
                    newPopulation.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, mutateFunction, shouldInitGenes: true));
                }
            }

            List<DNA<T>> tmpList = Population;
            Population = newPopulation;
            newPopulation = tmpList;

            Generation++;
        }

        private int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void CalculateFitness()
        {
            fitnessSum = 0;
            DNA<T> best = Population[0];

            for (int i = 0; i < Population.Count; i++)
            {
                fitnessSum += Population[i].CalculateFitness();

                if (Population[i].Fitness > best.Fitness)
                {
                    best = Population[i];
                }
            }

            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }

        private DNA<T> ChooseParent()
        {
            int randomNumber = random.Next(0, Elitism);

            return Population[randomNumber];
        }
    }
}
