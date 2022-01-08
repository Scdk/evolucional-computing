namespace backend.Models.Responses
{
    public class MaximizationOfFunctionResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
        public double FittestIndividualValueOfX { get; set; }
        public double FittestIndividualValueOfY { get; set; }
    }
}