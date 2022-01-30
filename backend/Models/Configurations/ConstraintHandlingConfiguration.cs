namespace backend.Models.Configurations
{
    public class ConstraintHandlingConfiguration
    {
        public int populationSize { get; set; }
        public double crossoverRate { get; set; }
        public double mutationRate { get; set; }
        public double elitismRate { get; set; }
        public double tournamentRate { get; set; }
        public double penaltyConstant { get; set; }
        public int numberOfVariables { get; set; }
        public double inferiorLimit { get; set; }
        public double superiorLimit { get; set; }
        public int numberOfGenerations { get; set; }
        public int challengeFlag { get; set; }
    }
}