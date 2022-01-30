namespace backend.Models.Responses
{
    public class ConstraintHandlingResponse
    {
        public int Generation { get; set; }
        public double FittestIndividualValue { get; set; }
        public double FittestIndividualConstraint { get; set; }

        public double GenerationAvarageValue { get; set; }
        public List<double> FittestIndividualVariables { get; set; } = new List<double>();
    }
}