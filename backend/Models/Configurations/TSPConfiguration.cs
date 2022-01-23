namespace backend.Models.Configurations
{
    public class TSPConfiguration
    {
        public int populationSize { get; set; }
        public double crossoverRate { get; set; }
        public double mutationRate { get; set; }
        public double elitismRate { get; set; }
        public double tournamentRate { get; set; }
        public int numberOfGenerations { get; set; }
        public int numberOfCities { get; set; }
    }
}