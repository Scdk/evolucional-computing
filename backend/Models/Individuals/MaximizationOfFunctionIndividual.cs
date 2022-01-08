namespace backend.Models.Individuals
{
    public class MaximizationOfFunctionIndividual
    {
        public MaximizationOfFunctionIndividual()
        {
            int number = new Random().Next(0,1024);
            binaryRepresentation = GenerateBinaryRepresentation(number);
        }
        public double realValueOfX
        { 
            get
            {
                float numericRepresentation = (float)Convert.ToInt32(this.binaryRepresentation.Substring(0, 5), 2);
                return numericRepresentation * 4/32;
            } 
            set
            {
                realValueOfX = value;
            }
        }
        public double realValueOfY
        { 
            get
            {
                float numericRepresentation = (float)Convert.ToInt32(this.binaryRepresentation.Substring(5), 2);
                return numericRepresentation * 2/32;
            } 
            set
            {
                realValueOfY = value;
            }
        }
        public string binaryRepresentation { get; set; }
        private string GenerateBinaryRepresentation(int number)
        {
            string binaryRepresentation = Convert.ToString(number, 2);
            while (binaryRepresentation.Length < 10)
            {
                binaryRepresentation = "0" + binaryRepresentation;
            } 
            return binaryRepresentation;
        }
    }
}