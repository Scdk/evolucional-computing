namespace backend.Models.Individuals
{
    public class ParameterVector
    {
        public List<double> Variables { get; set; } = new List<double>();

        #region Constructors

        public ParameterVector() { }

        public ParameterVector(int numberOfVariables, double infLimit, double supLimit)
        {
            for (int count = 0; count < numberOfVariables; count++)
            {
                Variables.Add(GenerateVariable(infLimit, supLimit));
            }
        }
        
        #endregion

        #region Methods

        public double GenerateVariable(double infLimit, double supLimit)
        {
            var rand = new Random();
            return ((supLimit - infLimit) * rand.NextDouble()) + infLimit;
        }
        
        public ParameterVector DeepCopy()
        {
            return (ParameterVector)this.MemberwiseClone();
        }

        #endregion
    }
}