using backend.Models.Individuals.TPS;

namespace backend.Models.Individuals
{
    public class TSPIndividual
    {
        public TSPIndividual() { }
        public TSPIndividual(List<City> listOfCities)
        {
            List<City> path = listOfCities.ToList();
            var rand = new Random();
            for (int i = path.Count - 1; 1 < i; i--)
            {
                int j = rand.Next(0, i);
                var temp = path[j];
                path[j] = path[i];
                path[i] = temp;
            }
            this.path = path.ToList();
        }
        public List<City> path { get; set; } = new List<City>();
            
    }
}