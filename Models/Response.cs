namespace Models.Response
{
    public class Response
    {
        public int Generation { get; set; }
        public double FittestIndividualValue { get; set; }
        public double FittestIndividualTargetFunctionValue { get; set; }
        public double FittestIndividualFitness { get; set; }
        public double GenerationAvarageFitness { get; set; }
    }
}