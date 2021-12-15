namespace Models.Individual
{
    public class Individual
    {
        public Individual()
        {
            int number = new Random().Next(0,1024);
            binaryRepresentation = GenerateBinaryRepresentation(number);
        }
        public double realValue 
        { 
            get
            {
                return (float)Convert.ToInt32(this.binaryRepresentation, 2) / 2;
            } 
            set
            {
                realValue = value;
            }
        }
        public string binaryRepresentation { get; set; }
        public double selectionProbability  { get; set; }

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