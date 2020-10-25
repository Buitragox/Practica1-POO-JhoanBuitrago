namespace AutoFarm.Robots
{
    public class Robot
    {
        public string Id { get; set; }
        public double Battery { get; set; }
        public double MaxBattery { get; set; }
        public bool Activated { get; set; }

        public Robot(string id)
        {
            Id = id;
            Battery = 1000;
            MaxBattery = 1000;
            Activated = true;
        }
    }
}