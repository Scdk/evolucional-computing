namespace backend.Models
{
    public class MaximizationOfFunctionResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
    }
}