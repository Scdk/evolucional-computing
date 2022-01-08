namespace backend.Models.Configurations
{
    public class BasicConfiguration
    {
        public int populationSize { get; set; }
        public double crossoverRate { get; set; }
        public double mutationRate { get; set; }
        public int numberOfGenerations { get; set; }
    }
}