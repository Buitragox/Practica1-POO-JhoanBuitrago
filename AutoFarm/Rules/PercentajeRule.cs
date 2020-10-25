using System;

namespace AutoFarm.Rules
{
    public class PercentajeRule : IRule
    {
        public bool CheckRule(object value)
        {
            double doubleValue;
            bool success = Double.TryParse(value.ToString(), out doubleValue);
            if(success)
            {
                return doubleValue >= 0 && doubleValue <= 99;
            }
            else
            {
                return false;
            }
        }
    }
}