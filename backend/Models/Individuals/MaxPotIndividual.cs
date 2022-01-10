using backend.Models.Individuals.MaxPot;

namespace backend.Models.Individuals
{
    public class MaxPotIndividual
    {
        public MaxPotIndividual()
        {
            machines = new List<Machine>{};
            machines.Add(new Machine(20, 2));
            machines.Add(new Machine(15, 2));
            machines.Add(new Machine(35, 1));
            machines.Add(new Machine(40, 1));
            machines.Add(new Machine(15, 1));
            machines.Add(new Machine(15, 1));
            machines.Add(new Machine(10, 1));

            do
            {
                intervals = new List<Interval>{};
                intervals.Add(new Interval(80, GenerateIntervalString()));
                intervals.Add(new Interval(90, GenerateIntervalString()));
                intervals.Add(new Interval(65, GenerateIntervalString()));
                intervals.Add(new Interval(70, GenerateIntervalString())); 
            } while(ValidateIndividual() == false);
            
        }
        public List<Machine> machines { get; set; }
        public List<Interval> intervals { get; set; }
        public string GenerateIntervalString()
        {
            int rnd = new Random().Next(0, 128);
            string intervalString = Convert.ToString(rnd, 2);
            while (intervalString.Length < this.machines.Count())
            {
                intervalString = "0" + intervalString;
            } 
            return intervalString;
        }
        public bool ValidateIndividual()
        {
            return ValidateInterval() && ValidateLiquidPotency();
        }
        private bool ValidateInterval()
        {
            int numOfIntervals = 0;
            for (int i = 0; i < this.machines.Count(); i++)
            {
                numOfIntervals = this.machines[i].numOfIntervals;
                if(numOfIntervals > 0)
                {
                    foreach (Interval interval in this.intervals)
                    {
                        if(interval.machinesActivated[i] == '0')
                            numOfIntervals--;
                    }
                    if (numOfIntervals > 0) return false;
                }
            }
            return true;
        }
        private bool ValidateLiquidPotency()
        {
            int pt = 150;
            for(int i = 0; i < this.intervals.Count(); i++)
            {
                int pp = 0;
                Interval interval = this.intervals[i];
                for (int j = 0; j < interval.machinesActivated.Count(); j++)
                {
                    if(interval.machinesActivated[j] == '0')
                        pp += machines[j].capacity;
                }
                int pl = pt - pp - interval.demandMaxPot;
                if (pl < 0) return false;
            }
            return true;
        }
    }
}