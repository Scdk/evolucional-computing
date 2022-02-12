namespace backend.Models.Configurations
{
    public class DifferentialEvolutionConfiguration
    {
        public int populationSize { get; set; }
        public double crossoverRate { get; set; }
        public int numberOfVariables { get; set; }
        public double inferiorLimit { get; set; }
        public double superiorLimit { get; set; }
        public double fValue { get; set; }
        public int numberOfGenerations { get; set; }

    }
}