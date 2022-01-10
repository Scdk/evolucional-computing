using System.Text.Json;
using backend.Models.Configurations;
using backend.Models.Individuals;
using backend.Models.Responses;

namespace backend.Services
{
    public class MaxPot
    {
        private List<MaxPotIndividual> population = new List<MaxPotIndividual>{};
        private BasicConfiguration config = new BasicConfiguration();

        public List<MaxPotResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<BasicConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new MaxPotIndividual());
            }
        }

        private int TargetFunction(int pt, int pp, int pd)
        {
            return pt - pp - pd;
        }

        private double FitnessFunction (MaxPotIndividual individual)
        {
            List<int> listOfPl = new List<int> {};
            int pt = 150;
            
            for(int i = 0; i < individual.intervals.Count(); i++)
            {
                int pp = 0;
                var interval = individual.intervals[i];
                for (int j = 0; j < interval.machinesActivated.Count(); j++)
                {
                    if(interval.machinesActivated[j] == '0')
                        pp += individual.machines[j].capacity;
                }
                listOfPl.Add(TargetFunction(pt, pp, interval.demandMaxPot));
            }
            foreach (var item in listOfPl)
            {
                Console.Write(item);
                Console.Write("\n");
            }
            Console.Write("\n");
            return listOfPl.Sum() - StandardDeviation(listOfPl);
        }

        private double StandardDeviation(List<int> listOfPl)
        {
            double result = 0;
            double avarage = listOfPl.Average();
            for (int count = 0; count < listOfPl.Count(); count++)
            {
                result += Math.Pow(listOfPl[count] - avarage, 2);
            }
            result = result/(listOfPl.Count() - 1);
            result = Math.Sqrt(result);
            return result;
        }

        private List<double> MakeListOfIndividualsFitness()
        {
            List<double> populationFitness = new List<double>{};
            foreach (var individual in population)
            {
                populationFitness.Add(FitnessFunction(individual));
            }
            return populationFitness;
        }

        private double GetGenerationAvarageFitness()
        {
            List<double> populationFitness = MakeListOfIndividualsFitness();
            return populationFitness.Sum() / populationFitness.Count;
        }

        private MaxPotIndividual FindFittestIndividualOfGeneration()
        {
            MaxPotIndividual fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration) < FitnessFunction(individual))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private MaxPotIndividual SelectIndividual()
        {
            List<MaxPotIndividual> randomIndividuals = new List<MaxPotIndividual>{ population[new Random().Next(0,config.populationSize)] };
            MaxPotIndividual selectedIndividual = randomIndividuals[0];
            for(int count = 1; count < 3; count++)
            {
                randomIndividuals.Add(population[new Random().Next(0,config.populationSize)]);
                if (FitnessFunction(selectedIndividual)
                        < FitnessFunction(randomIndividuals[count]))
                    selectedIndividual = randomIndividuals[count];
            }
            return selectedIndividual;
        }

        private void GenerateNewPopulation()
        {
            List<MaxPotIndividual> newGeneration = new List<MaxPotIndividual>{ FindFittestIndividualOfGeneration() };
            while(newGeneration.Count < config.populationSize)
            {
                MaxPotIndividual parent1 = SelectIndividual();
                MaxPotIndividual parent2 = SelectIndividual();
                MaxPotIndividual child1 = new MaxPotIndividual();
                MaxPotIndividual child2 = new MaxPotIndividual();
                for (int count = 0; count < parent1.intervals.Count(); count++)
                {
                    if (new Random().NextDouble() <= config.crossoverRate)
                    {
                        child1.intervals[count].machinesActivated = Crossover(parent1.intervals[count].machinesActivated,
                                                                                 parent2.intervals[count].machinesActivated);
                        child2.intervals[count].machinesActivated = Crossover(parent2.intervals[count].machinesActivated,
                                                                                 parent1.intervals[count].machinesActivated);
                    }
                    else
                    {
                        child1.intervals[count].machinesActivated = parent1.intervals[count].machinesActivated;
                        child2.intervals[count].machinesActivated = parent2.intervals[count].machinesActivated;
                    }

                    child1.intervals[count].machinesActivated = Mutation(child1.intervals[count].machinesActivated);
                    child2.intervals[count].machinesActivated = Mutation(child2.intervals[count].machinesActivated);
                    
                }
                if (child1.ValidateIndividual()) newGeneration.Add(child1);
                if (child2.ValidateIndividual()) newGeneration.Add(child2);
                
            }
            population = newGeneration;
        }

        private string Crossover(string parent1, string parent2)
        {
            int maxIndex = parent1.Length - 1;
            int index = new Random().Next(0, maxIndex);
            string individual = parent1.Substring(0, index) + parent2.Substring(index);
            return individual;
        }

        private string Mutation(string individual)
        {
            string newMachinesActivated = "";
            foreach (var character in individual)
            {
                if (new Random().NextDouble() <= config.mutationRate )
                {
                    if (character == '0') newMachinesActivated += '1';
                    else newMachinesActivated += '0';
                }
                else newMachinesActivated += character;
            }
            return newMachinesActivated;
        }

        private List<MaxPotResponse> GetResponses(int numberOfGenerations)
        {
            List<MaxPotResponse> reponses = new List<MaxPotResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                MaxPotIndividual fittestIndividual = FindFittestIndividualOfGeneration();
                reponses.Add(new MaxPotResponse{
                    Generation = count,
                    FittestIndividualFitness = FitnessFunction(fittestIndividual),
                    GenerationAvarageFitness = GetGenerationAvarageFitness(),
                    Scheduling = GetScheduling(fittestIndividual)
                });
                GenerateNewPopulation();
                
            }
            return reponses;
        }
        
        private List<String> GetScheduling(MaxPotIndividual individual)
        {
            List<String> result = new List<String>{};
            foreach (var interval in individual.intervals)
            {
                result.Add(interval.machinesActivated);
            }
            return result;
        }


        public void test(string configuration)
        {
            config = JsonSerializer.Deserialize<BasicConfiguration>(configuration);
            GenerateInitialPopulation();
            for (int count = 0; count < config.numberOfGenerations; count++)
            {
                MaxPotIndividual fittestIndividual = FindFittestIndividualOfGeneration();
                GenerateNewPopulation();
            }
        }
    }
}