using AutoFarm.Crops;

namespace AutoFarm.Lands
{
    public class Land
    {
        public string Id { get; set; }
        public Crop LandCrop { get; set; }
        public double Irrigation { get; set; }
        public double Fertility { get; set; }
        public bool IrrigatorBool { get; set; }
        public double IrrPercentaje { get; set; }
        public bool Used { get; set; }

        public Land(string id)
        {
            Id = id;
            Irrigation = 100;
            Fertility = 100;
            IrrigatorBool = true;
            IrrPercentaje = 20;
            LandCrop = new Crop();
            Used = false;
        }
    }
}