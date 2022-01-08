using System.Text.Json;
using backend.Models.Individuals;
using backend.Models.Configurations;
using backend.Models.Responses;

namespace backend.Services
{
    public class SimpleExemple
    {
        private List<Individual> population = new List<Individual>{};
        private BasicConfiguration config = new BasicConfiguration();

        public List<Response> main(string configuration)
        {
            config = JsonSerializer.Deserialize<BasicConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new Individual());
            }
            DetermineSelectionProbability();
        }

        private double TargetFuntion(double variable)
        {
            double targetFunction = - Math.Abs(variable * Math.Sin(Math.Sqrt(Math.Abs(variable))));
            double constant = 419.0;
            return targetFunction + constant;
        }

        private double FitnessFunction (double variable)
        {
            return 1/TargetFuntion(variable);
        }

        private List<Double> MakeListOfIndividualsFitness()
        {
            List<Double> populationFitness = new List<Double>{};
            foreach (var individual in population)
            {
                populationFitness.Add(FitnessFunction(individual.realValue));
            }
            return populationFitness;
        }

        private double GetGenerationAvarageFitness()
        {
            List<Double> populationFitness = MakeListOfIndividualsFitness();
            return populationFitness.Sum() / populationFitness.Count;
        }

        private Individual FindFittestIndividualOfGeneration()
        {
            Individual fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration.realValue) < FitnessFunction(individual.realValue))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private void DetermineSelectionProbability()
        {
            double generationTotalFitness = MakeListOfIndividualsFitness().Sum();
            foreach (var individual in population)
            {
                individual.selectionProbability = FitnessFunction(individual.realValue) / generationTotalFitness;
            }
        }

        private Individual SelectIndividual()
        {
            double randomNumber = new Random().NextDouble();
            double acumulativeSelectionProbability = 0.0;
            int index = 0;
            for (; index < (population.Count - 1); index++)
            {
                acumulativeSelectionProbability += population[index].selectionProbability;
                if(randomNumber < acumulativeSelectionProbability)
                {
                    return population[index];
                }
            }
            return population[index];
        }

        private Individual Crossover(Individual parent1, Individual parent2)
        {
            Individual individual = new Individual();
            int maxIndex = parent1.binaryRepresentation.Length - 1;
            int index = new Random().Next(0, maxIndex);
            individual.binaryRepresentation = parent1.binaryRepresentation.Substring(0, index) + parent2.binaryRepresentation.Substring(index);
            return individual;
        }

        private Individual Mutation(Individual individual)
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
            return individual;
        }

        private void GenerateNewPopulation()
        {
            List<Individual> newGeneration = new List<Individual>{};
            while(newGeneration.Count < config.populationSize)
            {
                Individual parent1 = SelectIndividual();
                Individual parent2 = SelectIndividual();
                Individual child1 = new Individual();
                Individual child2 = new Individual();
                if (new Random().NextDouble() <= config.crossoverRate)
                {
                    child1 = Crossover(parent1, parent2);
                    child2 = Crossover(parent2, parent1);

                    if (new Random().NextDouble() <= config.mutationRate ) newGeneration.Add(Mutation(child1));
                    else newGeneration.Add(child1);

                    if (new Random().NextDouble() <= config.mutationRate ) newGeneration.Add(Mutation(child2));
                    else newGeneration.Add(child2);
                }
            }
            population = newGeneration;
            DetermineSelectionProbability();
        }

        private List<Response> GetResponses(int numberOfGenerations)
        {
            List<Response> reponses = new List<Response> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                Individual fittestIndividual = FindFittestIndividualOfGeneration();
                reponses.Add(new Response{
                    Generation = count,
                    FittestIndividualValue = fittestIndividual.realValue,
                    FittestIndividualTargetFunctionValue = TargetFuntion(fittestIndividual.realValue),
                    FittestIndividualFitness = FitnessFunction(fittestIndividual.realValue),
                    GenerationAvarageFitness = GetGenerationAvarageFitness()
                });
                GenerateNewPopulation();
                
            }
            return reponses;
        }
    }
}