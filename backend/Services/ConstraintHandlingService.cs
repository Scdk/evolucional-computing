using System.Text.Json;
using backend.Models.Configurations;
using backend.Models.Individuals;
using backend.Models.Responses;

namespace backend.Services
{
    public class ConstraintHandling
    {
        private List<ContinuousParametersIndividual> population = new List<ContinuousParametersIndividual>();
        private ConstraintHandlingConfiguration config = new ConstraintHandlingConfiguration();
        public List<ConstraintHandlingResponse> main(string configuration)
        {
            config = JsonSerializer.Deserialize<ConstraintHandlingConfiguration>(configuration);
            if (config.challengeFlag == 1)
            {
                GenerateInitialPopulationChallenge();
            }
            else
            {
                GenerateInitialPopulation();
            }
            return GetResponses(config.numberOfGenerations);
        }

        private void GenerateInitialPopulation()
        {
            while (population.Count() < config.populationSize)
            {
                population.Add(new ContinuousParametersIndividual(config.numberOfVariables, config.inferiorLimit, config.superiorLimit));
            }
        }

        private double TargetFunction(List<double> variables)
        {
            return Math.Pow(variables[0] - 1, 2) + Math.Pow(variables[1] - 1, 2);
        }

        private double ConstraintFunction(List<double> variables)
        {
            double penality = Math.Pow(Math.Max(0, variables[0] + variables[1] - 0.5), 2);
            penality += Math.Pow(variables[0] - variables[1] - 2, 2);
            return config.penaltyConstant * penality;
        }

        private double FitnessFunction (ContinuousParametersIndividual individual)
        {
            if (config.challengeFlag == 1)
            {
                return Math.Pow(10, 9) - (TargetFunctionChallenge(individual.Variables) + ConstraintFunctionChallenge(individual.Variables));
            }
            else 
            {
                return Math.Pow(10, 9) - (TargetFunction(individual.Variables) + ConstraintFunction(individual.Variables));
            }
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

        private List<ConstraintHandlingResponse> GetResponses(int numberOfGenerations)
        {
            List<ConstraintHandlingResponse> reponses = new List<ConstraintHandlingResponse> {};
            for (int count = 0; count < numberOfGenerations; count++)
            {
                ContinuousParametersIndividual fittestIndividual = FindFittestIndividualOfGeneration().DeepCopy();
                if (config.challengeFlag == 1)
                {
                    reponses.Add(new ConstraintHandlingResponse
                    {
                        Generation = count,
                        FittestIndividualFitness = TargetFunctionChallenge(fittestIndividual.Variables),
                        FittestIndividualConstraint = ConstraintFunctionChallenge(fittestIndividual.Variables),
                        GenerationAvarageFitness = GetGenerationAvarageTargetValue(),
                        FittestIndividualVariables = fittestIndividual.Variables
                    });
                }
                else
                {
                    reponses.Add(new ConstraintHandlingResponse
                    {
                        Generation = count,
                        FittestIndividualFitness = TargetFunction(fittestIndividual.Variables),
                        FittestIndividualConstraint = ConstraintFunction(fittestIndividual.Variables),
                        GenerationAvarageFitness = GetGenerationAvarageTargetValue(),
                        FittestIndividualVariables = fittestIndividual.Variables
                    });
                }
                GenerateNewPopulation();
                
            }
            return reponses;
        }

        private double GetGenerationAvarageTargetValue()
        {
            var listOfTargetValue = new List<double>();
            foreach (var individual in population)
            {
                listOfTargetValue.Add(TargetFunction(individual.Variables));
            }
            return listOfTargetValue.Average();
        }

        #region Challenge

        private void GenerateInitialPopulationChallenge()
        {
            int[] list = {9, 10, 11};
            while (population.Count() < config.populationSize)
            {
                var individual = new ContinuousParametersIndividual();
                for (int count = 0; count < 13; count++)
                {
                    if (list.Contains(count))
                    {
                        individual.Variables.Add(individual.GenerateVariable(0, 100));
                    }
                    else
                    {
                        individual.Variables.Add(individual.GenerateVariable(0, 1));
                    }
                }
                population.Add(individual.DeepCopy());
            }
        }

        private double TargetFunctionChallenge(List<double> variables)
        {
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;

            for (int i = 0; i < 4; i++)
            {
                sum1 += variables[i];
            }
            sum1 *= 5;
            
            for (int i = 0; i < 4; i++)
            {
                sum2 += Math.Pow(variables[i], 2);
            }
            sum2 *= 5;

            for (int i = 4; i < 13; i++)
            {
                sum3 += variables[i];
            }

            return sum1 - sum2 - sum3; 
        }

        private double ConstraintFunctionChallenge(List<double> variables)
        {
            double penality = 0;
            
            penality += Math.Pow(Math.Max(0, 2*variables[0] + 2*variables[1] + variables[9] + variables[10] - 10), 2);
            penality += Math.Pow(Math.Max(0, 2*variables[0] + 2*variables[2] + variables[9] + variables[11] - 10), 2);
            penality += Math.Pow(Math.Max(0, 2*variables[1] + 2*variables[2] + variables[10] + variables[11] - 10), 2);

            penality += Math.Pow(Math.Max(0, -8*variables[0] + variables[9]), 2);
            penality += Math.Pow(Math.Max(0, -8*variables[1] + variables[10]), 2);
            penality += Math.Pow(Math.Max(0, -8*variables[2] + variables[11]), 2);

            penality += Math.Pow(Math.Max(0, -2*variables[3] - variables[4] + variables[9]), 2);
            penality += Math.Pow(Math.Max(0, -2*variables[5] - variables[6] + variables[10]), 2);
            penality += Math.Pow(Math.Max(0, -2*variables[7] - variables[8] + variables[11]), 2);

            return config.penaltyConstant * penality;
        }

        #endregion
    }
}