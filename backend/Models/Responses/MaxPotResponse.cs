namespace backend.Models.Responses
{
    public class MaxPotResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
        public List<String> Scheduling { get; set; }
    }
}