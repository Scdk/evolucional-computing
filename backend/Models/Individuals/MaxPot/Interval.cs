namespace backend.Models.Individuals.MaxPot
{
    public class Interval
    {
        public Interval(int _demandMaxPot, string _machinesActivated)
        {
            demandMaxPot = _demandMaxPot;
            machinesActivated = _machinesActivated;
        }
        public string machinesActivated { get; set; }
        public int demandMaxPot { get; set; }
    }
}