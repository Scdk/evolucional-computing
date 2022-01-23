using System.Text.Json;
using backend.Models.Configurations;
using backend.Models.Individuals;
using backend.Models.Individuals.TPS;
using backend.Models.Responses;

namespace backend.Services
{
    public class TSP
    {
        private const int MAX_SIZE = 1000;
        private List<TSPIndividual> population = new List<TSPIndividual>();
        private TSPConfiguration config = new TSPConfiguration();

        public List<TSPResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<TSPConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            var listOfCities = new List<City>();
            var rand = new Random();
            for (int count = 0; count < config.numberOfCities; count++)
            {
                listOfCities.Add(new City(rand.Next(0, MAX_SIZE), rand.Next(0, MAX_SIZE)));
            }
            while (population.Count() < config.populationSize)
            {
                population.Add(new TSPIndividual(listOfCities));
            }
        }

        private double TargetFuntion(City point1, City point2)
        {
            return Math.Sqrt(Math.Pow(point2.positionY - point1.positionY, 2)+Math.Pow(point2.positionX - point1.positionX, 2));
        }

        private double FitnessFunction (TSPIndividual individual)
        {
            var totalDistance = 0.0;
            for (int i = 1; i < individual.path.Count; i++)
            {
                totalDistance += this.TargetFuntion(individual.path[i-1], individual.path[i]);
            }
            return Math.Pow(10,6)/totalDistance;
        }

        private List<Double> MakeListOfIndividualsFitness()
        {
            List<Double> populationFitness = new List<Double>{};
            foreach (var individual in population)
            {
                populationFitness.Add(FitnessFunction(individual));
            }
            return populationFitness;
        }

        private double GetGenerationAvarageFitness()
        {
            List<Double> populationFitness = MakeListOfIndividualsFitness();
            return populationFitness.Sum() / populationFitness.Count;
        }

        private TSPIndividual FindFittestIndividualOfGeneration()
        {
            TSPIndividual fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration) < FitnessFunction(individual))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private TSPIndividual SelectIndividual()
        {
            List<TSPIndividual> randomIndividuals = new List<TSPIndividual>{ population[new Random().Next(0,config.populationSize)] };
            TSPIndividual selectedIndividual = randomIndividuals[0];
            if (config.tournamentRate >= 1) config.tournamentRate = 1;
            var individualsInTournament =  Math.Round(population.Count * config.tournamentRate);
            for(int count = 1; count < individualsInTournament; count++)
            {
                randomIndividuals.Add(population[new Random().Next(0,config.populationSize)]);
                if (FitnessFunction(selectedIndividual)
                        < FitnessFunction(randomIndividuals[count]))
                    selectedIndividual = randomIndividuals[count];
            }
            return selectedIndividual;
        }

        private List<TSPIndividual> GenerateElitistGeneration()
        {
            var elitistGeneration = new List<TSPIndividual>();
            var fittestIndividuals = population.ToList().OrderBy(c => FitnessFunction(c)).ToList();
            if (config.elitismRate >= 1) config.elitismRate = 1;
            var numberOfIndividuals =  Math.Round(population.Count * config.elitismRate);
            for(int count = 0; count < numberOfIndividuals; count++)
            {
                elitistGeneration.Add(fittestIndividuals[count]);
            }
            return elitistGeneration;
        }

        private City GetCorrectCity(TSPIndividual individual, TSPIndividual parent, int startIndex, int index)
        {
            var newIndex = individual.path.FindIndex(f => f.positionX == parent.path[index].positionX && f.positionY == parent.path[index].positionY);
            if (newIndex == -1)
            {
                return parent.path[index];
            }
            else
            {
                return GetCorrectCity(individual, parent, startIndex, newIndex + startIndex);
                // return GetCorrectCity(individual, parent, startIndex, newIndex + (2*startIndex)-individual.path.Count);
            }
        }

        private TSPIndividual Crossover(TSPIndividual parent1, TSPIndividual parent2, int subListStart, int subListSize)
        {
            TSPIndividual individual = new TSPIndividual();
            individual.path = parent2.path.GetRange(subListStart, subListSize);
            for (int i = 0; i < subListStart; i++)
            {
                individual.path.Insert(i, GetCorrectCity(individual, parent1, subListStart, i));
            }
            for (int i = subListStart + subListSize; i < parent1.path.Count; i++)
            {
                individual.path.Insert(i, GetCorrectCity(individual, parent1, 0, i));
            }
            return individual;
        }

        private TSPIndividual Mutation(TSPIndividual individual)
        {
            int maxIndex = individual.path.Count - 1;
            int index1 = new Random().Next(0, maxIndex);
            int index2 = new Random().Next(0, maxIndex);
            var temp = individual.path[index1];
            individual.path[index1] = individual.path[index2];
            individual.path[index2] = temp;
            return individual;
        }

        private void GenerateNewPopulation()
        {
            List<TSPIndividual> newGeneration = GenerateElitistGeneration();
            while(newGeneration.Count < config.populationSize)
            {
                TSPIndividual parent1 = SelectIndividual();
                TSPIndividual parent2 = SelectIndividual();
                TSPIndividual child1 = new TSPIndividual();
                TSPIndividual child2 = new TSPIndividual();
                if (new Random().NextDouble() <= config.crossoverRate)
                {
                    int maxIndex = parent1.path.Count - 1;
                    int subListStart = new Random().Next(0, maxIndex);
                    int subListSize = new Random().Next(subListStart, maxIndex) - subListStart;
                    Console.WriteLine(subListStart);
                    Console.WriteLine(subListSize);
                    Console.WriteLine("");
                    child1 = Crossover(parent1, parent2, subListStart, subListSize);
                    child2 = Crossover(parent2, parent1, subListStart, subListSize);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }

                newGeneration.Add(Mutation(child1));
                if(newGeneration.Count < config.populationSize) newGeneration.Add(Mutation(child2));
            }
            population = newGeneration;
        }

        private List<TSPResponse> GetResponses(int numberOfGenerations)
        {
            List<TSPResponse> reponses = new List<TSPResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                TSPIndividual fittestIndividual = FindFittestIndividualOfGeneration();
                reponses.Add(new TSPResponse{
                    Generation = count,
                    FittestIndividualFitness = FitnessFunction(fittestIndividual),
                    GenerationAvarageFitness = GetGenerationAvarageFitness(),
                    Path = GetPath(fittestIndividual)
                });
                GenerateNewPopulation();
                
            }
            return reponses;
        }

        private List<List<int>> GetPath(TSPIndividual fittestIndividual)
        {
            var path = new List<List<int>>();
            foreach (var city in fittestIndividual.path)
            {
                path.Add(new List<int>{city.positionX, city.positionY});
            }
            return path;
        }

        public void test(string configuration)
        {
            // TSPIndividual parent1 = new TSPIndividual();
            // parent1.path.Add(new City(1, 1));
            // parent1.path.Add(new City(2, 2));
            // parent1.path.Add(new City(3, 3));
            // parent1.path.Add(new City(4, 4));
            // parent1.path.Add(new City(5, 5));
            // parent1.path.Add(new City(6, 6));
            // parent1.path.Add(new City(7, 7));
            // parent1.path.Add(new City(8, 8));
            // TSPIndividual parent2 = new TSPIndividual();
            // parent2.path.Add(new City(3, 3));
            // parent2.path.Add(new City(7, 7));
            // parent2.path.Add(new City(5, 5));
            // parent2.path.Add(new City(1, 1));
            // parent2.path.Add(new City(6, 6));
            // parent2.path.Add(new City(8, 8));
            // parent2.path.Add(new City(2, 2));
            // parent2.path.Add(new City(4, 4));
            // TSPIndividual child1 = new TSPIndividual();
            // TSPIndividual child2 = new TSPIndividual();
            // int maxIndex = parent1.path.Count - 1;
            // int subListStart = 3;
            // int subListSize = 3;
            // child1 = Crossover(parent1, parent2, subListStart, subListSize);
            // child2 = Crossover(parent2, parent1, subListSize, subListStart);
            // Console.WriteLine("Eita bicho, cruzamento");
        }
    }
}