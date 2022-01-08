using System.Text.Json;
using backend.Models.Individuals;
using backend.Models.Configurations;
using backend.Models.Responses;

namespace backend.Services
{
    public class MaximizationOfFunction
    {
        private List<MaximizationOfFunctionIndividual> population = new List<MaximizationOfFunctionIndividual>{};
        private BasicConfiguration config = new BasicConfiguration();

        public List<MaximizationOfFunctionResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<BasicConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new MaximizationOfFunctionIndividual());
            }
        }

        private double TargetFuntion(double variable1, double variable2)
        {
            return 10 + (variable1 * Math.Sin(4 * variable1)) + (3 * Math.Sin(2 * variable2));
        }

        private double FitnessFunction (double variable1, double variable2)
        {
            return TargetFuntion(variable1, variable2);
        }

        private List<Double> MakeListOfIndividualsFitness()
        {
            List<Double> populationFitness = new List<Double>{};
            foreach (var individual in population)
            {
                populationFitness.Add(FitnessFunction(individual.realValueOfX, individual.realValueOfY));
            }
            return populationFitness;
        }

        private double GetGenerationAvarageFitness()
        {
            List<Double> populationFitness = MakeListOfIndividualsFitness();
            return populationFitness.Sum() / populationFitness.Count;
        }

        private MaximizationOfFunctionIndividual FindFittestIndividualOfGeneration()
        {
            MaximizationOfFunctionIndividual fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration.realValueOfX, fittestIndividualOfGeneration.realValueOfY) < FitnessFunction(individual.realValueOfX, individual.realValueOfY))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private MaximizationOfFunctionIndividual SelectIndividual()
        {
            List<MaximizationOfFunctionIndividual> randomIndividuals = new List<MaximizationOfFunctionIndividual>{ population[new Random().Next(0,config.populationSize)] };
            MaximizationOfFunctionIndividual selectedIndividual = randomIndividuals[0];
            for(int count = 1; count < 3; count++)
            {
                randomIndividuals.Add(population[new Random().Next(0,config.populationSize)]);
                if (FitnessFunction(selectedIndividual.realValueOfX, selectedIndividual.realValueOfY)
                        < FitnessFunction(randomIndividuals[count].realValueOfX, randomIndividuals[count].realValueOfY))
                    selectedIndividual = randomIndividuals[count];
            }
            return selectedIndividual;
        }

        private MaximizationOfFunctionIndividual Crossover(MaximizationOfFunctionIndividual parent1, MaximizationOfFunctionIndividual parent2)
        {
            MaximizationOfFunctionIndividual individual = new MaximizationOfFunctionIndividual();
            int maxIndex = parent1.binaryRepresentation.Length - 1;
            int index = new Random().Next(0, maxIndex);
            individual.binaryRepresentation = parent1.binaryRepresentation.Substring(0, index) + parent2.binaryRepresentation.Substring(index);
            return individual;
        }

        private MaximizationOfFunctionIndividual Mutation(MaximizationOfFunctionIndividual individual)
        {
            string newBinaryRepresentataion = "";
            foreach (var character in individual.binaryRepresentation)
            {
                if (new Random().NextDouble() <= config.mutationRate )
                {
                    if (character == '0') newBinaryRepresentataion += '1';
                    else newBinaryRepresentataion += '0';
                }
                else newBinaryRepresentataion += character;
            }
            individual.binaryRepresentation = newBinaryRepresentataion; 
            return individual;
        }

        private void GenerateNewPopulation()
        {
            List<MaximizationOfFunctionIndividual> newGeneration = new List<MaximizationOfFunctionIndividual>{ FindFittestIndividualOfGeneration() };
            while(newGeneration.Count < config.populationSize)
            {
                MaximizationOfFunctionIndividual parent1 = SelectIndividual();
                MaximizationOfFunctionIndividual parent2 = SelectIndividual();
                MaximizationOfFunctionIndividual child1 = new MaximizationOfFunctionIndividual();
                MaximizationOfFunctionIndividual child2 = new MaximizationOfFunctionIndividual();
                if (new Random().NextDouble() <= config.crossoverRate)
                {
                    child1 = Crossover(parent1, parent2);
                    child2 = Crossover(parent2, parent1);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }

                newGeneration.Add(Mutation(child1));
                newGeneration.Add(Mutation(child2));
            }
            population = newGeneration;
        }

        private List<MaximizationOfFunctionResponse> GetResponses(int numberOfGenerations)
        {
            List<MaximizationOfFunctionResponse> reponses = new List<MaximizationOfFunctionResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                MaximizationOfFunctionIndividual fittestIndividual = FindFittestIndividualOfGeneration();
                reponses.Add(new MaximizationOfFunctionResponse{
                    Generation = count,
                    FittestIndividualFitness = FitnessFunction(fittestIndividual.realValueOfX, fittestIndividual.realValueOfY),
                    GenerationAvarageFitness = GetGenerationAvarageFitness(),
                    FittestIndividualValueOfX = fittestIndividual.realValueOfX,
                    FittestIndividualValueOfY = fittestIndividual.realValueOfY
                });
                GenerateNewPopulation();
                
            }
            return reponses;
        }
    }
}