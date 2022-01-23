namespace backend.Models.Responses
{
    public class ContinuousParametersResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
        public List<double> FittestIndividualVariables { get; set; } = new List<double>();
    }
}