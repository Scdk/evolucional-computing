namespace backend.Models.Responses
{
    public class TSPResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
        public List<List<int>> Path { get; set; } = new List<List<int>>();
    }
}