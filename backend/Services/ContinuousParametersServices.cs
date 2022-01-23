using System.Text.Json;
using backend.Models.Configurations;
using backend.Models.Individuals;
using backend.Models.Responses;

namespace backend.Services
{
    public class ContinuousParameters
    {
        const int A = 10;
        private List<ContinuousParametersIndividual> population = new List<ContinuousParametersIndividual>();
        private ContinuousParametersConfiguration config = new ContinuousParametersConfiguration();

        public List<ContinuousParametersResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<ContinuousParametersConfiguration>(configuration);
            GenerateInitialPopulation();
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new ContinuousParametersIndividual(config.numberOfVariables, config.inferiorLimit, config.superiorLimit));
            }
        }

        private double TargetFuntion(List<double> variables)
        {
            var numOfVar = variables.Count;
            double sum = 0;
            for (int i = 0; i < numOfVar; i++)
            {
                sum += Math.Pow(variables[i], 2) - A*Math.Cos(2*Math.PI*variables[i]);
            }
            return A*numOfVar + sum;
        }

        private double FitnessFunction (ContinuousParametersIndividual individual)
        {
            return Math.Pow(10,2) - TargetFuntion(individual.Variables);
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

        private ContinuousParametersIndividual FindFittestIndividualOfGeneration()
        {
            ContinuousParametersIndividual fittestIndividualOfGeneration = population[0];
            foreach (var individual in population)
            {
                if (FitnessFunction(fittestIndividualOfGeneration) < FitnessFunction(individual))
                    fittestIndividualOfGeneration = individual;
            }
            return fittestIndividualOfGeneration;
        }

        private ContinuousParametersIndividual SelectIndividual()
        {
            List<ContinuousParametersIndividual> randomIndividuals = new List<ContinuousParametersIndividual>
            {
                population[new Random().Next(0,config.populationSize)] 
            };
            ContinuousParametersIndividual selectedIndividual = randomIndividuals[0];
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

        private List<ContinuousParametersIndividual> GenerateElitistGeneration()
        {
            var elitistGeneration = new List<ContinuousParametersIndividual>();
            var fittestIndividuals = population.ToList().OrderBy(c => -FitnessFunction(c)).ToList();
            if (config.elitismRate >= 1) config.elitismRate = 1;
            var numberOfIndividuals =  Math.Round(population.Count * config.elitismRate);
            for(int count = 0; count < numberOfIndividuals; count++)
            {
                elitistGeneration.Add(fittestIndividuals[count].DeepCopy());
            }
            return elitistGeneration.ToList();
        }

        private ContinuousParametersIndividual Crossover(ContinuousParametersIndividual parent1, ContinuousParametersIndividual parent2, double beta)
        {
            var individual = new ContinuousParametersIndividual();
            for (int i = 0; i < parent1.Variables.Count; i++)
            {
                individual.Variables.Add((beta*parent1.Variables[i]) + ((1-beta)*parent2.Variables[i]));
            }
            return individual;
        }

        private ContinuousParametersIndividual Mutation(ContinuousParametersIndividual individual)
        {
            var mutatedIndividual = new ContinuousParametersIndividual();
            for (int i = 0; i < individual.Variables.Count; i++)
            {
                if(new Random().NextDouble() <= config.mutationRate)
                {
                    mutatedIndividual.Variables.Add(individual.GenerateVariable(config.inferiorLimit, config.superiorLimit));
                }
                else
                {
                    mutatedIndividual.Variables.Add(individual.DeepCopy().Variables[i]);
                }
            }
            return mutatedIndividual;
        }

        private void GenerateNewPopulation()
        {
            List<ContinuousParametersIndividual> newGeneration = GenerateElitistGeneration();
            while(newGeneration.Count < config.populationSize)
            {
                ContinuousParametersIndividual parent1 = SelectIndividual().DeepCopy();
                ContinuousParametersIndividual parent2 = SelectIndividual().DeepCopy();
                ContinuousParametersIndividual child1 = new ContinuousParametersIndividual();
                ContinuousParametersIndividual child2 = new ContinuousParametersIndividual();
                if (new Random().NextDouble() <= config.crossoverRate)
                {
                    var rand = new Random();
                    child1 = Crossover(parent1, parent2, rand.NextDouble());
                    child2 = Crossover(parent2, parent1, rand.NextDouble());
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

        private List<ContinuousParametersResponse> GetResponses(int numberOfGenerations)
        {
            List<ContinuousParametersResponse> reponses = new List<ContinuousParametersResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                ContinuousParametersIndividual fittestIndividual = FindFittestIndividualOfGeneration().DeepCopy();
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