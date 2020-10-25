using AutoFarm.Events;

namespace AutoFarm.Crops
{
    public class Crop
    {
        public string Type { get; set; }
        public double Growth { get; set; }
        public double Edibility { get; set; }
        public bool EventBool { get; set; }
        public Event CurrentEvent { get; set; }

        public Crop()
        {
            Growth = 0;
            Edibility = 100;
            EventBool = false;
        }
    }
}