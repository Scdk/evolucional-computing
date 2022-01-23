namespace backend.Models.Individuals.TPS
{
    public class City
    {
        public City(int _positionX, int _positionY)
        {
            positionX = _positionX;
            positionY = _positionY;
        }

        public int positionX { get; set; }
        public int positionY { get; set; }
    }
}