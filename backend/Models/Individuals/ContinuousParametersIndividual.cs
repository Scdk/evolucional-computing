namespace backend.Models.Individuals
{
    public class ContinuousParametersIndividual
    {
        public ContinuousParametersIndividual() { }
        public ContinuousParametersIndividual(int numberOfVariables, double infLimit, double supLimit)
        {
            for (int count = 0; count < numberOfVariables; count++)
            {
                Variables.Add(GenerateVariable(infLimit, supLimit));
            }
        }
        
        public List<double> Variables { get; set; } = new List<double>();
        
        public double GenerateVariable(double infLimit, double supLimit)
        {
            var rand = new Random();
            return ((supLimit - infLimit) * rand.NextDouble()) + infLimit;
        }
        
        public ContinuousParametersIndividual DeepCopy()
        {
            return (ContinuousParametersIndividual)this.MemberwiseClone();
        }
    }
}