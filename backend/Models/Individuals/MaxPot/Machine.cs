namespace backend.Models.Individuals.MaxPot
{
    public class Machine
    {
        public Machine(int _capacity, int _numOfIntervals)
        {
            capacity = _capacity;
            numOfIntervals = _numOfIntervals;
        }

        public int capacity { get; set; }
        public int numOfIntervals { get; set; }
    }
}