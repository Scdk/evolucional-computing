using System.Text.Json;
using backend.Models.Configurations;
using backend.Models.Individuals;
using backend.Models.Responses;

namespace backend.Services
{
    public class DifferentialEvolutionService
    {
        private List<ParameterVector> population = new List<ParameterVector>();
        private DifferentialEvolutionConfiguration config = new DifferentialEvolutionConfiguration();

        public List<ContinuousParametersResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<DifferentialEvolutionConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new ParameterVector(config.numberOfVariables, config.inferiorLimit, config.superiorLimit));
            }
        }

        private double TargetFuntion(List<double> variables)
        {
            double result = 0;
            for (int i = 0; i < variables.Count-1; i++)
            {
                result += Math.Pow((1 - variables[i]) , 2) + 100*Math.Pow((variables[i+1] - Math.Pow(variables[i], 2)), 2);
            }
            return result;
        }

        private double FitnessFunction (ParameterVector individual)
        {
            return TargetFuntion(individual.Variables);
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

        private ParameterVector FindFittestIndividualOfGeneration()
        {
            ParameterVector fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration) > FitnessFunction(individual))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private ParameterVector Crossover(ParameterVector targetVector, ParameterVector mutatedVector)
        {
            var trialVector = new ParameterVector();
            var rand = new Random();
            var randIndex = rand.Next(0, targetVector.Variables.Count+1);
            
            for (int i = 0; i < targetVector.Variables.Count; i++)
            {
                if (rand.NextDouble() <= config.crossoverRate || i == randIndex)
                {
                    trialVector.Variables.Add(mutatedVector.Variables[i]);
                }
                else
                {
                    trialVector.Variables.Add(targetVector.Variables[i]);
                }
            }

            return trialVector;
        }

        private ParameterVector Mutation(ParameterVector individual)
        {
            var targetVector = new ParameterVector();
            for (int i = 0; i < individual.Variables.Count; i++)
            {
                var rand = new Random();
                List<int> indexes = new List<int>();
                while(indexes.Count < 3)
                {
                    var index = rand.Next(0, individual.Variables.Count);
                    indexes.Add(index);
                }
                targetVector.Variables.Add(
                    individual.Variables[indexes[0]]+(config.fValue*(individual.Variables[indexes[2]] - individual.Variables[indexes[1]])));
            }
            return targetVector;

        }

        private void GenerateNewPopulation()
        {
            List<ParameterVector> newGeneration = new List<ParameterVector>();
            foreach(var individual in population)
            {
                ParameterVector targetVector = individual.DeepCopy();
                ParameterVector mutatedVector = Mutation(targetVector);
                ParameterVector trialVector = Crossover(targetVector, mutatedVector);

                if(FitnessFunction(trialVector) < FitnessFunction(targetVector))
                {
                    newGeneration.Add(trialVector);
                }
                else
                {
                    newGeneration.Add(targetVector);
                }
            }
            population = newGeneration;
        }

        private List<ContinuousParametersResponse> GetResponses(int numberOfGenerations)
        {
            List<ContinuousParametersResponse> reponses = new List<ContinuousParametersResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                ParameterVector fittestIndividual = FindFittestIndividualOfGeneration().DeepCopy();
                reponses.Add(new ContinuousParametersResponse
                {
                    Generation = count,
                    FittestIndividualFitness = FitnessFunction(fittestIndividual),
                    GenerationAvarageFitness = GetGenerationAvarageFitness(),
                    FittestIndividualVariables = fittestIndividual.Variables
                });
                GenerateNewPopulation();
                
            }
            return reponses;
        }
    }
}