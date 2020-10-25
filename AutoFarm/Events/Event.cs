namespace AutoFarm.Events
{
    public class Event
    {
        public string Name { get; set; }
        public double Damage { get; set; }

        public Event(string name, double dmg)
        {
            Name = name;
            Damage = dmg;
        }
    }
}